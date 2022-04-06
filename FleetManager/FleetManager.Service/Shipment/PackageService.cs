using FleetManager.Common.Enums;
using FleetManager.Data.Constants;
using FleetManager.Data.Infrastructure;
using FleetManager.Dto.Common;
using FleetManager.Dto.Shipment;
using FleetManager.Service.Mapping;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FleetManager.Service.Shipment
{
    public class PackageService : ShipmentService
    {
        private readonly IRedisProvider _redisProvider;
        private readonly ILogger<PackageService> _logger;
        private readonly IWrongDeliveryService _wrongDeliveryService;

        public PackageService(IRedisProvider redisProvider, ILogger<PackageService> logger, IWrongDeliveryService wrongDeliveryService)
        {
            _redisProvider = redisProvider;
            _logger = logger;
            _wrongDeliveryService = wrongDeliveryService;
        }

        public override async Task<ResultDto<bool>> Add(BaseShipmentDto dto)
        {
            var shipmentKey = string.Format(Constant.Shipment.ShipmentByBarcodeKey, dto.Barcode);
            if (string.IsNullOrEmpty(dto.Barcode) || await _redisProvider.IsExistAsync(shipmentKey))
                return new ResultDto<bool>(ResultStatus.ValidationError);

            var shipment = DtoMapper.Mapper.Map<Data.Entities.Shipment>(dto);
            shipment.ShipmentState = (int)BagStatus.Created;
            shipment.ShipmentType = ShipmentType.Package;

            await _redisProvider.SetAsync(shipmentKey, shipment);

            var package = DtoMapper.Mapper.Map<Data.Entities.Package>(dto);
            var packageKey = string.Format(Constant.Shipment.PackageByBarcodeKey, dto.Barcode);

            await _redisProvider.SetAsync(packageKey, package);

            return new ResultDto<bool>(ResultStatus.Success, true);
        }

        public override async Task<ResultDto<ShipmentDeliveryResponseDto>> Delivery(ShipmentDeliveryRequestDto dto)
        {
            var result = new ResultDto<ShipmentDeliveryResponseDto>(ResultStatus.NotFound);
            var key = string.Format(Constant.Shipment.PackageByBarcodeKey, dto.Barcode);
            var package = await _redisProvider.GetAsync<Data.Entities.Package>(key);
            if (package == null)
                return result;

            if (package.DeliveryPointId != dto.DeliveryPointId)
            {
                _logger.LogWarning($"Wrong PackageDelivery => Barcode: {package.Barcode} - PackageDeliveryPoint: {package.DeliveryPointId} - TargetDeliveryPoint:{dto.DeliveryPointId}");
                await _wrongDeliveryService.AddAsync(new AddWrongDeliveryDto { Barcode = package.Barcode, DeliveryPointId = dto.DeliveryPointId });
            }

            package.PackageStatus = package.DeliveryPointId == dto.DeliveryPointId ? PackageStatus.Unloaded : PackageStatus.Loaded;
            await _redisProvider.SetAsync(key, package);
            result.Data = new ShipmentDeliveryResponseDto { Barcode = dto.Barcode, State = (int)package.PackageStatus };
            result.Status = ResultStatus.Success;

            return result;
        }

        public override async Task<ResultDto<ShipmentDetailDto>> GetDetail(string barcode)
        {
            var key = string.Format(Constant.Shipment.PackageByBarcodeKey, barcode);
            var package = await _redisProvider.GetAsync<Data.Entities.Package>(key);
            if (package == null)
                return new ResultDto<ShipmentDetailDto>(ResultStatus.NotFound);

            var result = new ResultDto<ShipmentDetailDto>(ResultStatus.Success);
            result.Data = DtoMapper.Mapper.Map<ShipmentDetailDto>(package);
            return result;
        }
    }
}

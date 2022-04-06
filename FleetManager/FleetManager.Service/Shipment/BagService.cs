using FleetManager.Common.Enums;
using FleetManager.Data.Constants;
using FleetManager.Data.Infrastructure;
using FleetManager.Dto.Common;
using FleetManager.Dto.Shipment;
using FleetManager.Service.Mapping;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FleetManager.Service.Shipment
{
    public class BagService : ShipmentService
    {
        private readonly IRedisProvider _redisProvider;
        private readonly ILogger<BagService> _logger;
        private readonly IWrongDeliveryService _wrongDeliveryService;

        public BagService(IRedisProvider redisProvider, ILogger<BagService> logger, IWrongDeliveryService wrongDeliveryService)
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
            shipment.ShipmentType = ShipmentType.Bag;

            await _redisProvider.SetAsync(shipmentKey, shipment);

            var bag = DtoMapper.Mapper.Map<Data.Entities.Bag>(dto);
            var bagKey = string.Format(Constant.Shipment.BagByBarcodeKey, dto.Barcode);

            await _redisProvider.SetAsync(bagKey, bag);

            return new ResultDto<bool>(ResultStatus.Success, true);
        }

        public override async Task<ResultDto<ShipmentDeliveryResponseDto>> Delivery(ShipmentDeliveryRequestDto dto)
        {
            var result = new ResultDto<ShipmentDeliveryResponseDto>(ResultStatus.NotFound);
            var key = string.Format(Constant.Shipment.BagByBarcodeKey, dto.Barcode);
            var bag = await _redisProvider.GetAsync<Data.Entities.Bag>(key);
            if (bag == null)
                return result;

            if (bag.DeliveryPointId != dto.DeliveryPointId)
            {
                _logger.LogWarning($"Wrong BagDelivery => Barcode: {bag.Barcode} - BagDeliveryPoint: {bag.DeliveryPointId} - TargetDeliveryPoint:{dto.DeliveryPointId}");
                await _wrongDeliveryService.AddAsync(new AddWrongDeliveryDto { Barcode = bag.Barcode, DeliveryPointId = dto.DeliveryPointId });
            }

            bag.BagStatus = bag.DeliveryPointId == dto.DeliveryPointId ? BagStatus.Unloaded : BagStatus.Loaded;
            bag.Packages?.ForEach(x =>
            {
                x.PackageStatus = bag.DeliveryPointId == dto.DeliveryPointId ? PackageStatus.Unloaded : PackageStatus.Loaded;
                var packageKey = string.Format(Constant.Shipment.PackageByBarcodeKey, x.Barcode);
                _redisProvider.SetAsync(packageKey, x);
            });

            await _redisProvider.SetAsync(key, bag);
            result.Data = new ShipmentDeliveryResponseDto { Barcode = dto.Barcode, State = (int)bag.BagStatus };
            result.Status = ResultStatus.Success;

            return result;
        }

        public override async Task<ResultDto<ShipmentDetailDto>> GetDetail(string barcode)
        {
            var key = string.Format(Constant.Shipment.BagByBarcodeKey, barcode);
            var bag = await _redisProvider.GetAsync<Data.Entities.Bag>(key);
            if (bag == null)
                return new ResultDto<ShipmentDetailDto>(ResultStatus.NotFound);

            var result = new ResultDto<ShipmentDetailDto>(ResultStatus.Success);
            result.Data = DtoMapper.Mapper.Map<ShipmentDetailDto>(bag);
            return result;
        }

        public async Task<ResultDto<bool>> AddPackageToBag(AddPackageToBagDto dto)
        {
            var packageKey = string.Format(Constant.Shipment.PackageByBarcodeKey, dto.Barcode);
            var package = await _redisProvider.GetAsync<Data.Entities.Package>(packageKey);
            if (package == null)
                return new ResultDto<bool>(ResultStatus.NotFound, false, "Package not found.");

            var bagKey = string.Format(Constant.Shipment.BagByBarcodeKey, dto.BagBarcode);
            var bag = await _redisProvider.GetAsync<Data.Entities.Bag>(bagKey);
            if (bag == null)
                return new ResultDto<bool>(ResultStatus.NotFound, false, "Bag not found.");

            bag.Packages = bag.Packages ?? new List<Data.Entities.Package>();
            bag.Packages.Add(package);

            await _redisProvider.SetAsync(bagKey, bag);
            return new ResultDto<bool>(ResultStatus.Success, true);
        }
    }
}

using FleetManager.Common.Enums;
using FleetManager.Data.Constants;
using FleetManager.Data.Infrastructure;
using FleetManager.Dto.Common;
using FleetManager.Service.Mapping;
using FleetManager.Service.Shipment.Commands;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FleetManager.Service.Shipment.Handlers
{
    public class AddPackageCommandHandler : IRequestHandler<AddPackageCommand, ResultDto<bool>>
    {
        private readonly IRedisProvider _redisProvider;

        public AddPackageCommandHandler(IRedisProvider redisProvider)
        {
            _redisProvider = redisProvider;
        }

        public async Task<ResultDto<bool>> Handle(AddPackageCommand request, CancellationToken cancellationToken)
        {
            var shipmentKey = string.Format(Constant.Shipment.ShipmentByBarcodeKey, request.AddPackageRequest.Barcode);
            if (string.IsNullOrEmpty(request.AddPackageRequest.Barcode) || await _redisProvider.IsExistAsync(shipmentKey))
                return new ResultDto<bool>(ResultStatus.ValidationError);

            var shipment = DtoMapper.Mapper.Map<Data.Entities.Shipment>(request.AddPackageRequest);
            shipment.ShipmentState = (int)BagStatus.Created;
            shipment.ShipmentType = ShipmentType.Package;

            await _redisProvider.SetAsync(shipmentKey, shipment);

            var package = DtoMapper.Mapper.Map<Data.Entities.Package>(request.AddPackageRequest);
            var packageKey = string.Format(Constant.Shipment.PackageByBarcodeKey, request.AddPackageRequest.Barcode);

            await _redisProvider.SetAsync(packageKey, package);

            return new ResultDto<bool>(ResultStatus.Success, true);
        }
    }
}

using FleetManager.Common.Enums;
using FleetManager.Data.Constants;
using FleetManager.Data.Infrastructure;
using FleetManager.Dto.Common;
using FleetManager.Dto.Shipment;
using FleetManager.Service.Shipment.Queries;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FleetManager.Service.Shipment.Handlers
{
    public class GetShipmentDetailByBarcodeHandler : IRequestHandler<GetShipmentDetailByBarcodeQuery, ResultDto<ShipmentDetailDto>>
    {
        private readonly IRedisProvider _redisProvider;
        private readonly ServiceResolver _serviceResolver;

        public GetShipmentDetailByBarcodeHandler(IRedisProvider redisProvider, ServiceResolver serviceResolver)
        {
            _redisProvider = redisProvider;
            _serviceResolver = serviceResolver;
        }

        public async Task<ResultDto<ShipmentDetailDto>> Handle(GetShipmentDetailByBarcodeQuery request, CancellationToken cancellationToken)
        {
            var shipmentKey = string.Format(Constant.Shipment.ShipmentByBarcodeKey, request.Barcode);
            var shipment = await _redisProvider.GetAsync<Data.Entities.Shipment>(shipmentKey);
            if (shipment == null)
            {
                return new ResultDto<ShipmentDetailDto>(ResultStatus.NotFound);
            }

            var service = _serviceResolver(shipment.ShipmentType);
            var result = await service.GetDetail(request.Barcode);

            return result;
        }
    }
}

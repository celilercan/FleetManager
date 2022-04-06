using FleetManager.Common.Enums;
using FleetManager.Data.Constants;
using FleetManager.Data.Infrastructure;
using FleetManager.Dto.Common;
using FleetManager.Dto.Shipment;
using FleetManager.Service.Shipment.Commands;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FleetManager.Service.Shipment.Handlers
{
    public class ShipmentDeliveryCommandHandler : IRequestHandler<ShipmentDeliveryRequestCommand, ResultDto<ShipmentDeliveryResponseDto>>
    {
        private readonly IRedisProvider _redisProvider;
        private readonly ServiceResolver _serviceResolver;

        public ShipmentDeliveryCommandHandler(IRedisProvider redisProvider, ServiceResolver serviceResolver)
        {
            _redisProvider = redisProvider;
            _serviceResolver = serviceResolver;
        }

        public async Task<ResultDto<ShipmentDeliveryResponseDto>> Handle(ShipmentDeliveryRequestCommand request, CancellationToken cancellationToken)
        {
            var shipmentKey = string.Format(Constant.Shipment.ShipmentByBarcodeKey, request.ShipmentDeliveryRequest.Barcode);
            var shipment = await _redisProvider.GetAsync<Data.Entities.Shipment>(shipmentKey);
            if (shipment == null)
            {
                return new ResultDto<ShipmentDeliveryResponseDto>(ResultStatus.NotFound);
            }

            var service = _serviceResolver(shipment.ShipmentType);
            var result = await service.Delivery(request.ShipmentDeliveryRequest);

            if (result.IsSuccess)
            {
                shipment.ShipmentState = result.Data.State;
                await _redisProvider.SetAsync(shipmentKey, shipment); 
            }

            return result;
        }
    }
}

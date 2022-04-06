using FleetManager.Dto.Common;
using FleetManager.Dto.Shipment;
using MediatR;

namespace FleetManager.Service.Shipment.Commands
{
    public class ShipmentDeliveryRequestCommand : IRequest<ResultDto<ShipmentDeliveryResponseDto>>
    {
        public ShipmentDeliveryRequestDto ShipmentDeliveryRequest { get; set; }

        public ShipmentDeliveryRequestCommand(ShipmentDeliveryRequestDto shipmentDeliveryRequest)
        {
            ShipmentDeliveryRequest = shipmentDeliveryRequest;
        }
    }
}

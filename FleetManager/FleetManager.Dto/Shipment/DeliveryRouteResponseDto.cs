using System.Collections.Generic;

namespace FleetManager.Dto.Shipment
{
    public class DeliveryRouteResponseDto
    {
        public DeliveryRouteResponseDto()
        {
            Deliveries = new List<ShipmentDeliveryResponseDto>();
        }

        public int DeliveryPoint { get; set; }

        public List<ShipmentDeliveryResponseDto> Deliveries { get; set; }
    }
}

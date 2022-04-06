using System.Collections.Generic;

namespace FleetManager.Dto.Shipment
{
    public class DeliveryRouteRequestDto
    {
        public int DeliveryPoint { get; set; }

        public List<BarcodeRequestDto> Deliveries { get; set; }
    }
}

using System.Collections.Generic;

namespace FleetManager.Dto.Shipment
{
    public class DeliveryRequestDto
    {
        public string Plate { get; set; }

        public List<DeliveryRouteRequestDto> Route { get; set; }
    }
}

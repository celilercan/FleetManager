using System.Collections.Generic;

namespace FleetManager.Dto.Shipment
{
    public class DeliveryResponseDto
    {
        public DeliveryResponseDto()
        {
            Route = new List<DeliveryRouteResponseDto>();
        }

        public string Plate { get; set; }

        public List<DeliveryRouteResponseDto> Route { get; set; }
    }
}

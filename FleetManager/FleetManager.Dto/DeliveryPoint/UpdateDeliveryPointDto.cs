using System;

namespace FleetManager.Dto.DeliveryPoint
{
    public class UpdateDeliveryPointDto
    {
        public Guid Id { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }
    }
}

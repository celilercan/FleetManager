using System;

namespace FleetManager.Dto.DeliveryPoint
{
    public class DeliveryPointDetailDto
    {
        public Guid Id { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

using System;

namespace FleetManager.Dto.Vehicle
{
    public class VehicleDetailDto
    {
        public Guid Id { get; set; }
        public string LicencePlate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

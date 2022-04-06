using System;

namespace FleetManager.Dto.Vehicle
{
    public class UpdateVehicleDto
    {
        public Guid Id { get; set; }
        public string LicencePlate { get; set; }
    }
}

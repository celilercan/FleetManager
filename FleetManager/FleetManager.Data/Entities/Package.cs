using FleetManager.Common.Enums;

namespace FleetManager.Data.Entities
{
    public class Package : BaseEntity 
    {
        public Package()
        {
            this.PackageStatus = PackageStatus.Created;
        }
        public int DeliveryPointId { get; set; }
        public string Barcode { get; set; }
        public int Weight { get; set; }
        public PackageStatus PackageStatus { get; set; }
    }
}

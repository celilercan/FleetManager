using FleetManager.Common.Enums;
using System.Collections.Generic;

namespace FleetManager.Data.Entities
{
    public class Bag : BaseEntity
    {
        public Bag()
        {
            this.BagStatus = BagStatus.Created;
        }

        public int DeliveryPointId { get; set; }
        public string Barcode { get; set; }
        public List<Package> Packages { get; set; }
        public BagStatus BagStatus { get; set; }
    }
}

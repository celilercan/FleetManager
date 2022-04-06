using FleetManager.Common.Enums;

namespace FleetManager.Data.Entities
{
    public class Shipment : BaseEntity 
    {
        public string VehicleLicencePlate { get; set; }
        public string Barcode { get; set; }
        public int DeliveryPointId { get; set; }
        public int ShipmentState { get; set; }
        public ShipmentType ShipmentType { get; set; }
    }
}

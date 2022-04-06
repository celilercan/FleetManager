namespace FleetManager.Dto.Shipment
{
    public class ShipmentDetailDto
    {
        public int DeliveryPointId { get; set; }
        public string Barcode { get; set; }
        public int ShipmentState { get; set; }
    }
}

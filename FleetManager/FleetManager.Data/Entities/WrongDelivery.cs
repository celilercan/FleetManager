namespace FleetManager.Data.Entities
{
    public class WrongDelivery : BaseEntity
    {
        public int DeliveryPointId { get; set; }
        public string Barcode { get; set; }
    }
}

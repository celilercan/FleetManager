using System;

namespace FleetManager.Dto.Shipment
{
    public class WrongDeliveryDetailDto
    {
        public Guid Id { get; set; }
        public int DeliveryPointId { get; set; }
        public string Barcode { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

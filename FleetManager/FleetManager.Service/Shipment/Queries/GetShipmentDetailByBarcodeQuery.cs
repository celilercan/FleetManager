using FleetManager.Dto.Common;
using FleetManager.Dto.Shipment;
using MediatR;

namespace FleetManager.Service.Shipment.Queries
{
    public class GetShipmentDetailByBarcodeQuery : IRequest<ResultDto<ShipmentDetailDto>>
    {
        public string Barcode { get; set; }

        public GetShipmentDetailByBarcodeQuery(string barcode)
        {
            this.Barcode = barcode;
        }
    }
}

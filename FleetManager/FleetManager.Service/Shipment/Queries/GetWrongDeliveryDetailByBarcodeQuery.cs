using FleetManager.Dto.Common;
using FleetManager.Dto.Shipment;
using MediatR;

namespace FleetManager.Service.Shipment.Queries
{
    public class GetWrongDeliveryDetailByBarcodeQuery : IRequest<ResultDto<WrongDeliveryDetailDto>>
    {
        public string Barcode { get; set; }

        public GetWrongDeliveryDetailByBarcodeQuery(string barcode)
        {
            this.Barcode = barcode;
        }
    }
}

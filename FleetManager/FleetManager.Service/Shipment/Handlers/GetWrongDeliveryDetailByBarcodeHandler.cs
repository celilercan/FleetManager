using FleetManager.Dto.Common;
using FleetManager.Dto.Shipment;
using FleetManager.Service.Shipment.Queries;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FleetManager.Service.Shipment.Handlers
{
    public class GetWrongDeliveryDetailByBarcodeHandler : IRequestHandler<GetWrongDeliveryDetailByBarcodeQuery, ResultDto<WrongDeliveryDetailDto>>
    {
        private readonly IWrongDeliveryService _wrongDeliveryService;

        public GetWrongDeliveryDetailByBarcodeHandler(IWrongDeliveryService wrongDeliveryService)
        {
            _wrongDeliveryService = wrongDeliveryService;
        }

        public async Task<ResultDto<WrongDeliveryDetailDto>> Handle(GetWrongDeliveryDetailByBarcodeQuery request, CancellationToken cancellationToken)
        {
            var result = await _wrongDeliveryService.GetDetail(request.Barcode);

            return result;
        }
    }
}

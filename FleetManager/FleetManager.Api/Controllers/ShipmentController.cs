using FleetManager.Common.Enums;
using FleetManager.Dto.Common;
using FleetManager.Dto.Shipment;
using FleetManager.Service.Shipment;
using FleetManager.Service.Shipment.Commands;
using FleetManager.Service.Shipment.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace FleetManager.Api.Controllers
{
    public class ShipmentController : BaseController
    {
        private readonly IMediator _mediator;

        public ShipmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("addPackage")]
        [ProducesResponseType(typeof(ActionResult<ResultDto<bool>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ActionResult<ResultDto<bool>>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ActionResult<ResultDto<bool>>), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResultDto<bool>>> AddPackage([FromBody] AddPackageDto model)
        {
            var result = await _mediator.Send(new AddPackageCommand(model));
            return HttpResult(result);
        }

        [HttpPost("addBag")]
        [ProducesResponseType(typeof(ActionResult<ResultDto<bool>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ActionResult<ResultDto<bool>>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ActionResult<ResultDto<bool>>), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResultDto<bool>>> AddBag([FromBody] AddBagDto model)
        {
            var result = await _mediator.Send(new AddBagCommand(model));
            return HttpResult(result);
        }

        [HttpPut("addPackageToBag")]
        [ProducesResponseType(typeof(ActionResult<ResultDto<bool>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ActionResult<ResultDto<bool>>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ActionResult<ResultDto<bool>>), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResultDto<bool>>> AddPackageToBag([FromBody] AddPackageToBagDto model)
        {
            var result = await _mediator.Send(new AddPackageToBagCommand(model));
            return HttpResult(result);
        }

        [HttpPost("delivery")]
        [ProducesResponseType(typeof(ActionResult<ResultDto<DeliveryResponseDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ActionResult<ResultDto<DeliveryResponseDto>>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ActionResult<ResultDto<DeliveryResponseDto>>), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResultDto<bool>>> Delivery([FromBody] DeliveryRequestDto model)
        {
            var result = new ResultDto<DeliveryResponseDto> { Data = new DeliveryResponseDto { Plate = model.Plate} };

            foreach (var route in model.Route)
            {
                var routeResponse = new DeliveryRouteResponseDto { DeliveryPoint = route.DeliveryPoint };

                foreach (var item in route.Deliveries)
                {
                    var req = new ShipmentDeliveryRequestDto { DeliveryPointId = route.DeliveryPoint, Barcode = item.Barcode };
                    var deliveryResult = await _mediator.Send(new ShipmentDeliveryRequestCommand(req));
                    routeResponse.Deliveries.Add(deliveryResult.Data);
                }

                result.Data.Route.Add(routeResponse);
                result.Status = ResultStatus.Success;
            }

            return HttpResult(result);
        }

        [HttpGet("detail/{barcode}")]
        [ProducesResponseType(typeof(ActionResult<ResultDto<ShipmentDetailDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ActionResult<ResultDto<BaseShipmentDto>>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ActionResult<ResultDto<BaseShipmentDto>>), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResultDto<bool>>> GetDetail(string barcode)
        {
            var result = await _mediator.Send(new GetShipmentDetailByBarcodeQuery(barcode));

            return HttpResult(result);
        }

        [HttpGet("wrongDelivery/{barcode}")]
        [ProducesResponseType(typeof(ActionResult<ResultDto<WrongDeliveryDetailDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ActionResult<ResultDto<WrongDeliveryDetailDto>>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ActionResult<ResultDto<WrongDeliveryDetailDto>>), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResultDto<WrongDeliveryDetailDto>>> GetWrongDelivery(string barcode)
        {
            var result = await _mediator.Send(new GetWrongDeliveryDetailByBarcodeQuery(barcode));

            return HttpResult(result);
        }
    }
}

using FleetManager.Dto.Common;
using FleetManager.Dto.DeliveryPoint;
using FleetManager.Service.DeliveryPoint;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace FleetManager.Api.Controllers
{
    public class DeliveryPointController : BaseController
    {
        private IDeliveryPointService _deliveryPointService;

        public DeliveryPointController(IDeliveryPointService deliveryPointService)
        {
            _deliveryPointService = deliveryPointService;
        }

        [HttpGet("{index}")]
        [ProducesResponseType(typeof(ActionResult<ResultDto<DeliveryPointDetailDto>>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ResultDto<DeliveryPointDetailDto>>> GetById(int index)
        {
            var result = await _deliveryPointService.GetByIndexAsync(index);
            return HttpResult(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ActionResult<ResultDto<DeliveryPointDetailDto>>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ResultDto<DeliveryPointDetailDto>>> Post([FromBody] AddDeliveryPointDto model)
        {
            var result = await _deliveryPointService.AddAsync(model);
            return HttpResult(result);
        }

        [HttpPut]
        [ProducesResponseType(typeof(ActionResult<ResultDto<DeliveryPointDetailDto>>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ResultDto<DeliveryPointDetailDto>>> Update([FromBody] UpdateDeliveryPointDto model)
        {
            var result = await _deliveryPointService.UpdateAsync(model);
            return HttpResult(result);
        }

        [HttpDelete("{index}")]
        [ProducesResponseType(typeof(ActionResult<ResultDto<bool>>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ResultDto<bool>>> Delete(int index)
        {
            var result = await _deliveryPointService.DeleteAsync(index);
            return HttpResult(result);
        }
    }
}

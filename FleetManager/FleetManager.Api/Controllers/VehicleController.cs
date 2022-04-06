using System;
using System.Net;
using System.Threading.Tasks;
using FleetManager.Common.Enums;
using FleetManager.Dto.Common;
using FleetManager.Dto.Vehicle;
using FleetManager.Service.Vehicle;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FleetManager.Api.Controllers
{
    public class VehicleController : BaseController
    {
        private readonly ILogger<VehicleController> _logger;
        private readonly IVehicleService _vehicleService;

        public VehicleController(ILogger<VehicleController> logger, IVehicleService vehicleService)
        {
            _logger = logger;
            _vehicleService = vehicleService;
        }

        [HttpGet("{plate}")]
        [ProducesResponseType(typeof(ActionResult<ResultDto<VehicleDetailDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ActionResult<ResultDto<VehicleDetailDto>>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ActionResult<ResultDto<VehicleDetailDto>>), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResultDto<VehicleDetailDto>>> GetByPlate(string plate)
        {
            var result = await _vehicleService.GetByLicencePlateAsync(plate);
            return HttpResult(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ActionResult<ResultDto<VehicleDetailDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ActionResult<ResultDto<VehicleDetailDto>>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ActionResult<ResultDto<VehicleDetailDto>>), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResultDto<VehicleDetailDto>>> Post([FromBody] AddVehicleDto model)
        {
            var result = await _vehicleService.AddAsync(model);
            return HttpResult(result);
        }

        [HttpPut]
        [ProducesResponseType(typeof(ActionResult<ResultDto<VehicleDetailDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ActionResult<ResultDto<VehicleDetailDto>>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ActionResult<ResultDto<VehicleDetailDto>>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ActionResult<ResultDto<VehicleDetailDto>>), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResultDto<VehicleDetailDto>>> Update([FromBody] UpdateVehicleDto model)
        {
            var result = await _vehicleService.UpdateAsync(model);
            return HttpResult(result);
        }

        [HttpDelete("{plate}")]
        [ProducesResponseType(typeof(ActionResult<ResultDto<VehicleDetailDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ActionResult<ResultDto<VehicleDetailDto>>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ActionResult<ResultDto<VehicleDetailDto>>), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResultDto<VehicleDetailDto>>> Delete(string plate)
        {
            var result = await _vehicleService.DeleteAsync(plate);
            return HttpResult(result);
        }
    }
}

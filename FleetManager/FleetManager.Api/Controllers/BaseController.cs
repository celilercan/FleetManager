using FleetManager.Dto.Common;
using FleetManager.Common.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FleetManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        protected ActionResult HttpResult<T>(ResultDto<T> result)
        {
            switch (result.Status)
            {
                case ResultStatus.Success:
                    return Ok(result);
                case ResultStatus.NotFound:
                    return NotFound(result);
                case ResultStatus.ValidationError:
                    return BadRequest(result);
                case ResultStatus.Exception:
                    return StatusCode(StatusCodes.Status500InternalServerError, result);
                default:
                    return NotFound(result);
            }
        }
    }
}

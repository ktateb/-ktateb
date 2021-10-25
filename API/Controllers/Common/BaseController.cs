using System;
using System.Threading.Tasks;
using Common.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Common
{

    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        public ActionResult GetResult<T>(ResultService<T> result)
        {
            return result.Code switch
            {
                ResultStatusCode.Ok => Ok(result),
                ResultStatusCode.BadRequest => BadRequest(result),
                ResultStatusCode.NotFound => NotFound(result),
                ResultStatusCode.Unauthorized => Unauthorized(result),
                _ => StatusCode(500, result),
            };
        }
    }
}
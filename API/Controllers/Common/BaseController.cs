using System;
using Microsoft.AspNetCore.Mvc;
using Services.Services;

namespace API.Controllers.Common
{

    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        public ActionResult<T> GetResult<T>(ResultService<T> result)
        {

            if (result.Code == ResultStatusCode.Ok)
            {
                    return Ok(result.Result); 
            }
            if (result.Code == ResultStatusCode.BadRequist)
            {
                return BadRequest(result.Messege);
            }
            if (result.Code == ResultStatusCode.NotFound)
            {
                return NotFound(result.Messege);
            }
            return Unauthorized(result.Messege);
        }
    }
}
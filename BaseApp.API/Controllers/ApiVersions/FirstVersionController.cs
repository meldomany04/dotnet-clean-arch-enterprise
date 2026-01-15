using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BaseApp.API.Controllers.ApiVersions
{
    [ApiVersion("1.0")]
    [Route("api/version-manager")]
    [ApiController]
    public class FirstVersionController : ControllerBase
    {
        [HttpGet("method-version")]
        public IActionResult Get() => Ok("This is the first version of the API.");
    }
}

using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BaseApp.API.Controllers.ApiVersions
{
    [ApiVersion("2.0")]
    [Route("api/version-manager")]
    [ApiController]
    public class SecondVersionController : ControllerBase
    {
        [HttpGet("method-version")]
        public IActionResult Get() => Ok("This is the second version of the API.");
    }
}

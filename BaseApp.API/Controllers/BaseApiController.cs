using BaseApp.Application.Common.Responses;
using Microsoft.AspNetCore.Mvc;

namespace BaseApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        protected IActionResult OkResponse<T>(T data, string? message = null)
            => Ok(BaseResponse<T>.Ok(data, message));

        protected IActionResult FailResponse(string message)
            => BadRequest(BaseResponse<object>.Fail(message));
    }

}

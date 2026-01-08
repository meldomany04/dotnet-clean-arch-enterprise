using BaseApp.API.Models;
using BaseApp.Application.Common.Exceptions;
using BaseApp.Application.Common.Responses;
using Serilog;
using System.Text.Json;

namespace BaseApp.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;

        public ExceptionHandlingMiddleware(RequestDelegate next, IHostEnvironment env)
        {
            _next = next;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/json";

                var problem = new
                {
                    type = "https://httpstatuses.com/400",
                    title = "Validation Error",
                    status = 400,
                    errors = ex.Errors
                };

                var response = new BaseResponse<object>
                {
                    Success = false,
                    Message = "Validation failed",
                    Data = problem
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
            catch (AppException ex)
            {
                context.Response.StatusCode = ex.StatusCode;
                context.Response.ContentType = "application/json";

                var response = new BaseResponse<object>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = new
                    {
                        type = $"https://httpstatuses.com/{ex.StatusCode}",
                        status = ex.StatusCode
                    }
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
            catch (Exception ex)
            {
                Log.Error(ex,
                    "Unhandled exception at {Method} {Path}",
                    context.Request.Method,
                    context.Request.Path);

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var response = new BaseResponse<object>
                {
                    Success = false,
                    Message = "An unexpected error occurred.",
                    Data = _env.IsDevelopment() ? new { Exception = ex.ToString() } : null
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }

        private async Task WriteResponseAsync(
            HttpContext context,
            int statusCode,
            string message,
            string? details)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var response = new ErrorResponse
            {
                StatusCode = statusCode,
                Message = message,
                Details = details
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }

    }
}
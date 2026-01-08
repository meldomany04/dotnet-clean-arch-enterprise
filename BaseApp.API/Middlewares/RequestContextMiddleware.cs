using System.Diagnostics;
using System.Text;

namespace BaseApp.API.Middlewares
{
    public class RequestContextMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Items["Audit_StartTime"] = Stopwatch.StartNew();
            context.Items["Audit_Path"] = context.Request.Path;
            context.Items["Audit_Method"] = context.Request.Method;
            context.Items["Audit_Query_String"] = context.Request.QueryString.Value;


            string? requestBody = await ReadRequestBodyAsync(context);
            context.Items["Audit_Request_Body"] = requestBody;

            await _next(context);
        }

        private async Task<string?> ReadRequestBodyAsync(HttpContext context)
        {
            if (!context.Request.ContentLength.HasValue ||
                context.Request.ContentLength == 0)
                return null;

            context.Request.EnableBuffering();

            using var reader = new StreamReader(
                context.Request.Body,
                Encoding.UTF8,
                leaveOpen: true);

            var body = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;

            return body;
        }
    }
}

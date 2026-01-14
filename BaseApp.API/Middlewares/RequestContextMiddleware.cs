using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace BaseApp.API.Middlewares
{
    public class RequestContextMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;

        public RequestContextMiddleware(RequestDelegate next, IHostEnvironment env)
        {
            _next = next;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            context.Items["Audit_StartTime"] = stopwatch;
            context.Items["Audit_Path"] = context.Request.Path;
            context.Items["Audit_Method"] = context.Request.Method;
            context.Items["Audit_Query_String"] = context.Request.QueryString.Value;

            if (context.Request.Method == "POST" ||
                context.Request.Method == "PUT" ||
                context.Request.Method == "PATCH")
            {

               await ExtractRowVersionFromBody(context);
            }

            if (context.Request.Method == "DELETE")
            {
                ExtractRowVersionFromQueryOrHeaders(context);
            }

                await _next(context);
        }

        private async Task ExtractRowVersionFromBody(HttpContext context)
        {
            context.Request.EnableBuffering();

            using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;

            if (!string.IsNullOrEmpty(body))
            {
                context.Items["Audit_Request_Body"] = body;
                try
                {
                    using var jsonDoc = JsonDocument.Parse(body);
                    var rowVersionProperty = jsonDoc.RootElement.EnumerateObject()
                        .FirstOrDefault(p =>
                            p.Name.EndsWith("RowVersion", StringComparison.OrdinalIgnoreCase) ||
                            p.Name.Equals("RowVersion", StringComparison.OrdinalIgnoreCase));

                    if (!rowVersionProperty.Equals(default(JsonProperty)))
                    {
                        ExtractAndStoreRowVersion(context, rowVersionProperty.Value);
                    }
                }
                catch (JsonException)
                {
                }
            }
        }

        private void ExtractRowVersionFromQueryOrHeaders(HttpContext context)
        {
            if (context.Request.Query.TryGetValue("rowVersion", out var rowVersionQuery))
            {
                var rowVersionString = rowVersionQuery.FirstOrDefault();
                if (!string.IsNullOrEmpty(rowVersionString))
                {
                    try
                    {
                        var rowVersion = Convert.FromBase64String(rowVersionString);
                        context.Items["RowVersion"] = rowVersion;
                        return;
                    }
                    catch (FormatException)
                    {
                    }
                }
            }
        }

        private void ExtractAndStoreRowVersion(HttpContext context, JsonElement value)
        {
            if (value.ValueKind == JsonValueKind.String)
            {
                var rowVersionString = value.GetString();
                if (!string.IsNullOrEmpty(rowVersionString))
                {
                    try
                    {
                        var rowVersion = Convert.FromBase64String(rowVersionString);
                        context.Items["RowVersion"] = rowVersion;
                    }
                    catch (FormatException)
                    {
                    }
                }
            }
            else if (value.ValueKind == JsonValueKind.Array)
            {
                try
                {
                    var rowVersion = value.EnumerateArray()
                        .Select(e => e.GetByte())
                        .ToArray();
                    context.Items["RowVersion"] = rowVersion;
                }
                catch (InvalidOperationException)
                {
                }
            }
        }
    }
}

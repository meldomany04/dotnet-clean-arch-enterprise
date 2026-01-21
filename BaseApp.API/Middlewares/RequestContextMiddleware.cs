using System.Diagnostics;
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
                    var rowVersionMap = new Dictionary<string, Queue<byte[]>>(StringComparer.OrdinalIgnoreCase);

                    ExtractRowVersionsRecursive(jsonDoc.RootElement, rowVersionMap);

                    if (rowVersionMap.Any())
                    {
                        context.Items["RowVersionMap"] = rowVersionMap;
                    }
                }
                catch (JsonException)
                {
                }
            }
        }

        private void ExtractRowVersionsRecursive(JsonElement element, Dictionary<string, Queue<byte[]>> rowVersionMap)
        {
            if (element.ValueKind == JsonValueKind.Object)
            {
                foreach (var property in element.EnumerateObject())
                {
                    if (property.Name.EndsWith("RowVersion", StringComparison.OrdinalIgnoreCase))
                    {
                        var entityName = ExtractEntityName(property.Name);

                        if (TryExtractRowVersion(property.Value, out var rowVersion))
                        {
                            if (!rowVersionMap.ContainsKey(entityName))
                            {
                                rowVersionMap[entityName] = new Queue<byte[]>();
                            }
                            rowVersionMap[entityName].Enqueue(rowVersion);
                        }
                    }
                    else
                    {
                        ExtractRowVersionsRecursive(property.Value, rowVersionMap);
                    }
                }
            }
            else if (element.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in element.EnumerateArray())
                {
                    ExtractRowVersionsRecursive(item, rowVersionMap);
                }
            }
        }

        private string ExtractEntityName(string propertyName)
        {
            var entityName = propertyName.Substring(0,
                propertyName.Length - "RowVersion".Length);

            if (string.IsNullOrEmpty(entityName))
            {
                return "default";
            }

            return entityName.ToLowerInvariant();
        }

        private bool TryExtractRowVersion(JsonElement value, out byte[] rowVersion)
        {
            rowVersion = null;

            if (value.ValueKind == JsonValueKind.String)
            {
                var rowVersionString = value.GetString();
                if (!string.IsNullOrEmpty(rowVersionString))
                {
                    try
                    {
                        rowVersion = Convert.FromBase64String(rowVersionString);
                        return true;
                    }
                    catch (FormatException)
                    {
                        return false;
                    }
                }
            }
            else if (value.ValueKind == JsonValueKind.Array)
            {
                try
                {
                    rowVersion = value.EnumerateArray()
                        .Select(e => e.GetByte())
                        .ToArray();
                    return true;
                }
                catch (InvalidOperationException)
                {
                    return false;
                }
            }

            return false;
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
                        var rowVersionMap = new Dictionary<string, Queue<byte[]>>(StringComparer.OrdinalIgnoreCase)
                        {
                            ["default"] = new Queue<byte[]>(new[] { rowVersion })
                        };
                        context.Items["RowVersionMap"] = rowVersionMap;
                    }
                    catch (FormatException)
                    {
                    }
                }
            }
        }
    }
}
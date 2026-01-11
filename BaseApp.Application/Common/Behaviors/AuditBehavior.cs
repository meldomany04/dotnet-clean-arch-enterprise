using BaseApp.Application.Common.Auditing;
using BaseApp.Application.Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace BaseApp.Application.Common.Behaviors
{
    public class AuditBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IAuditLogger _auditLogger;
        private readonly IHttpContextAccessor _httpContext;

        public AuditBehavior(IAuditLogger auditLogger, IHttpContextAccessor httpContext)
        {
            _auditLogger = auditLogger;
            _httpContext = httpContext;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var context = _httpContext.HttpContext!;
            var stopwatch = context.Items["Audit_StartTime"] as Stopwatch ?? Stopwatch.StartNew();

            string? exception = null;
            bool success = true;
            int statusCode = StatusCodes.Status200OK;

            try
            {
                var response = await next();

                stopwatch.Stop();
                await LogAuditAsync(context, stopwatch, statusCode, success, exception);

                return response;
            }
            catch (AppException ex)
            {
                success = false;
                statusCode = ex.StatusCode;
                exception = ex.ToString();

                stopwatch.Stop();
                await LogAuditAsync(context, stopwatch, statusCode, success, exception);

                throw;
            }
            catch (Exception ex)
            {
                success = false;
                statusCode = StatusCodes.Status500InternalServerError;
                exception = ex.ToString();

                stopwatch.Stop();
                await LogAuditAsync(context, stopwatch, statusCode, success, exception);

                throw;
            }
        }

        private async Task LogAuditAsync(
            HttpContext context,
            Stopwatch stopwatch,
            int statusCode,
            bool success,
            string? exception)
        {

            var audit = new AuditLogEntry
            {
                HttpMethod = context.Items["Audit_Method"]?.ToString(),
                Path = context.Items["Audit_Path"]?.ToString(),
                StatusCode = statusCode,
                IsSuccess = success,
                UserId = "_currentUser.UserId",
                UserName = "_currentUser.UserName",
                Exception = exception,
                DurationMs = stopwatch.ElapsedMilliseconds,
                CorrelationId = context.TraceIdentifier,
                RequestBody = context.Items["Audit_Request_Body"]?.ToString(),
                QueryString = context.Items["Audit_Query_String"]?.ToString()
            };

            try
            {
                await _auditLogger.LogAsync(audit);
            }
            catch (Exception auditEx)
            {
            }
        }

    }

}

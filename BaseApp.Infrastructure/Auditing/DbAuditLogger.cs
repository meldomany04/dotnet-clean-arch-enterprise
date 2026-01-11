using BaseApp.Application.Common.Auditing;
using BaseApp.Infrastructure.Persistence;
using BaseApp.Infrastructure.Persistence.Entities;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace BaseApp.Infrastructure.Auditing
{
    public class DbAuditLogger : IAuditLogger
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public DbAuditLogger(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task LogAsync(AuditLogEntry entry)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                using var scope = _scopeFactory.CreateScope();
                
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                
                var log = new AuditLog
                {
                    HttpMethod = entry.HttpMethod,
                    Path = entry.Path,
                    QueryString = entry.QueryString,
                    StatusCode = entry.StatusCode,
                    IsSuccess = entry.IsSuccess,
                    UserId = entry.UserId,
                    UserName = entry.UserName,
                    RequestBody = entry.RequestBody,
                    ResponseBody = entry.ResponseBody,
                    Exception = entry.Exception,
                    CorrelationId = entry.CorrelationId,
                    DurationMs = entry.DurationMs,
                    CreatedAtUtc = DateTime.UtcNow
                };

                context.AuditLogs.Add(log);

                var saved = await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
            }
        }
    }

}

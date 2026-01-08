using BaseApp.Application.Common.Auditing;
using BaseApp.Infrastructure.Persistence;
using BaseApp.Infrastructure.Persistence.Entities;

namespace BaseApp.Infrastructure.Auditing
{
    public class DbAuditLogger : IAuditLogger
    {
        private readonly AppDbContext _context;

        public DbAuditLogger(AppDbContext context)
        {
            _context = context;
        }

        public async Task LogAsync(AuditLogEntry entry)
        {
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

            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();
        }
    }

}

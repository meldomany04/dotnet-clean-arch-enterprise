namespace BaseApp.Application.Common.Auditing
{
    public interface IAuditLogger
    {
        Task LogAsync(AuditLogEntry entry);
    }
}

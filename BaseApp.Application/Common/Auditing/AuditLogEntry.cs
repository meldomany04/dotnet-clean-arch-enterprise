namespace BaseApp.Application.Common.Auditing
{
    public class AuditLogEntry
    {
        public string HttpMethod { get; set; } = default!;
        public string Path { get; set; } = default!;
        public string? QueryString { get; set; }

        public int StatusCode { get; set; }
        public bool IsSuccess { get; set; }

        public string? UserId { get; set; }
        public string? UserName { get; set; }

        public string? RequestBody { get; set; }
        public string? ResponseBody { get; set; }

        public string? Exception { get; set; }

        public string? CorrelationId { get; set; }
        public long DurationMs { get; set; }
    }

}

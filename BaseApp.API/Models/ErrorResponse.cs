namespace BaseApp.API.Models
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = default!;
        public string? Details { get; set; }
    }
}

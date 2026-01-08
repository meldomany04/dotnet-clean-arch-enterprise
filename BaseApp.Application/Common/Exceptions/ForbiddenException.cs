namespace BaseApp.Application.Common.Exceptions
{
    public class ForbiddenException : AppException
    {
        public ForbiddenException(string message = "Forbidden")
            : base(message) { }

        public override int StatusCode => 403;
    }
}

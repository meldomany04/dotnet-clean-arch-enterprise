namespace BaseApp.Application.Common.Exceptions
{
    public class UnauthorizedException : AppException
    {
        public UnauthorizedException(string message = "Unauthorized")
            : base(message) { }

        public override int StatusCode => 401;
    }

}

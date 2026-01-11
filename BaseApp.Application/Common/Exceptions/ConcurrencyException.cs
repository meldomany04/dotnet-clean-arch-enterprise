namespace BaseApp.Application.Common.Exceptions
{
    public class ConcurrencyException : AppException
    {
        public ConcurrencyException(string message)
            : base(message)
        {
        }

        public override int StatusCode => 409;
    }

}

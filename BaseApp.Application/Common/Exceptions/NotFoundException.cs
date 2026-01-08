namespace BaseApp.Application.Common.Exceptions
{
    public class NotFoundException : AppException
    {
        public NotFoundException(string entity, object key)
            : base($"{entity} with key '{key}' was not found.") { }

        public override int StatusCode => 404;
    }

}

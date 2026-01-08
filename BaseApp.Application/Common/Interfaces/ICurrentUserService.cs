namespace BaseApp.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
        string? UserName { get; }
        string? Email { get; }
        string? DisplayName { get; }
        string? FirstName { get; }
        string? LastName { get; }
        IEnumerable<string> Roles { get; }
        bool IsAuthenticated { get; }
    }
}

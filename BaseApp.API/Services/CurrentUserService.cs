using BaseApp.Application.Common.Interfaces;
using System.Security.Claims;

namespace BaseApp.API.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _context;

        public CurrentUserService(IHttpContextAccessor context)
        {
            _context = context;
        }

        private ClaimsPrincipal? User => _context.HttpContext?.User;

        public string? UserId =>
            User?.FindFirst("sub")?.Value
            ?? User?.FindFirst("Id")?.Value;

        public string? UserName =>
            User?.FindFirst("Username")?.Value
            ?? User?.FindFirst(ClaimTypes.Name)?.Value;

        public string? Email =>
            User?.FindFirst("Email")?.Value
            ?? User?.FindFirst(ClaimTypes.Email)?.Value;

        public string? DisplayName =>
            User?.FindFirst("DisplayName")?.Value;

        public string? FirstName =>
            User?.FindFirst("FirstName")?.Value
            ?? User?.FindFirst(ClaimTypes.GivenName)?.Value;

        public string? LastName =>
            User?.FindFirst("LastName")?.Value
            ?? User?.FindFirst(ClaimTypes.Surname)?.Value;

        public IEnumerable<string> Roles =>
            User?.FindAll(ClaimTypes.Role)
                .Select(r => r.Value)
            ?? Enumerable.Empty<string>();

        public bool IsAuthenticated =>
            User?.Identity?.IsAuthenticated ?? false;
    }

}

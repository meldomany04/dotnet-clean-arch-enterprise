using BaseApp.Domain.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BaseApp.Infrastructure.Persistence.Interceptors
{
    public class ConcurrencyInterceptor : SaveChangesInterceptor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ConcurrencyInterceptor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            ExtractRowVersionFromContext(eventData.Context);
            return result;
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            ExtractRowVersionFromContext(eventData.Context);
            return ValueTask.FromResult(result);
        }

        private void ExtractRowVersionFromContext(DbContext context)
        {
            if (context == null || _httpContextAccessor.HttpContext == null) return;

            if (_httpContextAccessor.HttpContext.Items.TryGetValue("RowVersion", out var rowVersionObj))
            {
                if (rowVersionObj is byte[] rowVersion)
                {
                    ApplyRowVersionToModifiedEntities(context, rowVersion);
                }
                return;
            }
        }

        private void ApplyRowVersionToModifiedEntities(DbContext context, byte[] rowVersion)
        {
            foreach (var entry in context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified && e.Entity is BaseEntity))
            {
                var property = entry.Property(nameof(BaseEntity.RowVersion));
                if (property != null)
                {
                    property.OriginalValue = rowVersion;
                }
            }
        }
    }
}

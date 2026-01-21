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
            ApplyRowVersionsToModifiedEntities(eventData.Context);
            return result;
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            ApplyRowVersionsToModifiedEntities(eventData.Context);
            return ValueTask.FromResult(result);
        }

        private void ApplyRowVersionsToModifiedEntities(DbContext context)
        {
            if (context == null || _httpContextAccessor.HttpContext == null)
                return;

            if (!_httpContextAccessor.HttpContext.Items.TryGetValue("RowVersionMap", out var rowVersionMapObj))
                return;

            if (rowVersionMapObj is not Dictionary<string, Queue<byte[]>> rowVersionMap)
                return;

            var modifiedEntities = context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified && e.Entity is BaseEntity)
                .ToList();

            foreach (var entry in modifiedEntities)
            {
                var entityTypeName = entry.Entity.GetType().Name.ToLowerInvariant();
                var rowVersionProperty = entry.Property(nameof(BaseEntity.RowVersion));

                if (rowVersionProperty == null)
                    continue;

                byte[] rowVersionToApply = null;

                if (rowVersionMap.TryGetValue(entityTypeName, out var queue) && queue.Any())
                {
                    rowVersionToApply = queue.Dequeue();
                }
                else if (entityTypeName.EndsWith("s"))
                {
                    var singularName = entityTypeName.TrimEnd('s');
                    if (rowVersionMap.TryGetValue(singularName, out queue) && queue.Any())
                    {
                        rowVersionToApply = queue.Dequeue();
                    }
                }
                else if (rowVersionMap.TryGetValue("default", out queue) && queue.Any())
                {
                    rowVersionToApply = queue.Dequeue();
                }

                if (rowVersionToApply != null)
                {
                    rowVersionProperty.OriginalValue = rowVersionToApply;
                }
            }
        }
    }
}
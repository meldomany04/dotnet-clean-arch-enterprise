using BaseApp.Domain.Entities;
using BaseApp.Infrastructure.Persistence.Entities;
using BaseApp.Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace BaseApp.Infrastructure.Persistence
{
    public class AppDbContext : BaseDbContext
    {
        private readonly ConcurrencyInterceptor _concurrencyInterceptor;

        public AppDbContext(DbContextOptions<AppDbContext> options, ConcurrencyInterceptor concurrencyInterceptor) : base(options)
        {
            _concurrencyInterceptor = concurrencyInterceptor;
        }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(_concurrencyInterceptor);
        }

    }

}
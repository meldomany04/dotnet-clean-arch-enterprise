using BaseApp.Domain.Entities;
using BaseApp.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace BaseApp.Infrastructure.Persistence
{
    public class AppDbContext : BaseDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    }

}
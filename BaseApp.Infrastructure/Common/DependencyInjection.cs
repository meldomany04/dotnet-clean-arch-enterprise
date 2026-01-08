using BaseApp.Application.Common.Auditing;
using BaseApp.Application.Common.BackgroundJobs;
using BaseApp.Application.Common.Interfaces;
using BaseApp.Application.Common.Realtime;
using BaseApp.Infrastructure.Auditing;
using BaseApp.Infrastructure.BackgroundJobs;
using BaseApp.Infrastructure.Persistence;
using BaseApp.Infrastructure.Realtime;
using BaseApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BaseApp.Infrastructure.Common
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IEmailBackgroundJob, EmailBackgroundJob>();

            services.AddSignalR();
            services.AddScoped<INotificationHub, NotificationHubAdapter>();

            services.AddScoped<IAuditLogger, DbAuditLogger>();

            return services;
        }
    }

}

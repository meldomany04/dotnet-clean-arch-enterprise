using Hangfire;

namespace BaseApp.API.Extentions
{
    public static class HangfireServicesExtention
    {
        public static IServiceCollection ConfigureHangfire(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(config =>
            {
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                      .UseSimpleAssemblyNameTypeSerializer()
                      .UseRecommendedSerializerSettings()
                      .UseSqlServerStorage(
                          configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddHangfireServer();

            return services;
        }

    }
}

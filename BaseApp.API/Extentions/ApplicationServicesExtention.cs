using BaseApp.Application.Common;
using BaseApp.Infrastructure.Common;

namespace BaseApp.API.Extentions
{
    public static class ApplicationServicesExtention
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplication();
            services.AddInfrastructure(configuration);

            return services;
        }

    }
}

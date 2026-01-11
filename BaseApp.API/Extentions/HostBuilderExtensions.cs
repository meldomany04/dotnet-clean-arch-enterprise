using Serilog;

namespace BaseApp.API.Extentions
{
    public static class HostBuilderExtensions
    {
        public static WebApplicationBuilder ConfigureSerilog(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((context, services, configuration) =>
            {
                configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext();
            });

            return builder;
        }
    }
}

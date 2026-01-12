using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace BaseApp.API.Extentions
{
    public static class LocalizationExtentions
    {
        public static IServiceCollection ConfigureLocalization(this IServiceCollection services)
        {
            services.AddLocalization();

            var supportedCultures = new[]
            {
                new CultureInfo("en"),
                new CultureInfo("ar"),
                new CultureInfo("fr")
            };

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders = new[]
                {
                new AcceptLanguageHeaderRequestCultureProvider()
            };
            });

            return services;
        }

    }
}

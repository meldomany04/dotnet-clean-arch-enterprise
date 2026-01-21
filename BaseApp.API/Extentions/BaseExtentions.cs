using Asp.Versioning;
using Asp.Versioning.Conventions;
using BaseApp.API.Common;
using BaseApp.API.Services;
using BaseApp.Application.Common.Interfaces;
using FluentValidation.AspNetCore;

namespace BaseApp.API.Extentions
{
    public static class BaseExtentions
    {
        public static IServiceCollection ConfigureBaseServices(this IServiceCollection services)
        {
            services.AddControllers();
            //services.AddFluentValidationAutoValidation();
            services.AddEndpointsApiExplorer();

            services.AddApiVersioning(
                options =>
                {
                    options.DefaultApiVersion = new ApiVersion(1.0);
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.ReportApiVersions = true;
                    options.ApiVersionReader = ApiVersionReader.Combine(
                           new UrlSegmentApiVersionReader(),
                           new QueryStringApiVersionReader("api-version"),
                           new HeaderApiVersionReader("X-Version"),
                           new MediaTypeApiVersionReader("x-version"));
                })
            .AddMvc(
                options =>
                {
                    options.Conventions.Add(new VersionByNamespaceConvention());
                })
            .AddApiExplorer(setup =>
            {
                setup.GroupNameFormat = "'v'VVV";
                setup.SubstituteApiVersionInUrl = true;
            });
            services.AddSwaggerGen();
            services.ConfigureOptions<NamedSwaggerGenOptions>();

            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            return services;
        }
    }
}

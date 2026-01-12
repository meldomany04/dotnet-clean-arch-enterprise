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
            services.AddFluentValidationAutoValidation();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            return services;
        }
    }
}

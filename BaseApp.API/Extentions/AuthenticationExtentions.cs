using BaseApp.API.Common;
using Microsoft.IdentityModel.Tokens;

namespace BaseApp.API.Extentions
{
    public static class AuthenticationExtentions
    {
        public static IServiceCollection ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var signingKey = JwtKeyProvider.GetSigningKeyAsync().GetAwaiter().GetResult();

            services
                .AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "http://localhost:7000";
                    options.RequireHttpsMetadata = false;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = "http://localhost:7000",
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = signingKey,
                        NameClaimType = "Username",
                        RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
                    };
                });

            services.AddAuthorization();

            return services;
        }

    }
}

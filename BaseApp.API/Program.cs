using BaseApp.API.Middlewares;
using BaseApp.API.Services;
using BaseApp.Application.Common;
using BaseApp.Application.Common.Interfaces;
using BaseApp.Infrastructure.Common;
using BaseApp.Infrastructure.Realtime;
using Hangfire;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Security.Cryptography;
using System.Text.Json;



async Task<RsaSecurityKey> GetSigningKeyAsync()
{
    using var httpClient = new HttpClient();
    var jwksJson = await httpClient.GetStringAsync("http://localhost:7000/.well-known/openid-configuration/jwks");
    var jwks = JsonDocument.Parse(jwksJson);
    var key = jwks.RootElement.GetProperty("keys")[0];

    var rsa = new RSAParameters
    {
        Modulus = Base64UrlEncoder.DecodeBytes(key.GetProperty("n").GetString()),
        Exponent = Base64UrlEncoder.DecodeBytes(key.GetProperty("e").GetString())
    };

    var rsaKey = new RsaSecurityKey(rsa)
    {
        KeyId = key.GetProperty("kid").GetString()
    };

    return rsaKey;
}

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext();
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var signingKey = await GetSigningKeyAsync();

builder.Services
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

        //options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
        //{
        //    OnTokenValidated = context =>
        //    {
        //        Console.WriteLine("Token validated successfully");
        //        return Task.CompletedTask;
        //    },
        //    OnAuthenticationFailed = context =>
        //    {
        //        Console.WriteLine($"Authentication failed: {context.Exception.Message}");
        //        return Task.CompletedTask;
        //    }
        //};
    });

builder.Services.AddAuthorization();

builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();


builder.Services.AddHangfire(config =>
{
    config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
          .UseSimpleAssemblyNameTypeSerializer()
          .UseRecommendedSerializerSettings()
          .UseSqlServerStorage(
              builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddHangfireServer();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});


var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestContextMiddleware>();

app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseHangfireDashboard("/hangfire");

app.MapHub<NotificationHub>("/hubs/notifications");

app.Run();

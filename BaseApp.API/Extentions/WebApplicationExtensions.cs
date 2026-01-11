using BaseApp.API.Middlewares;
using BaseApp.Infrastructure.Realtime;
using Hangfire;
using Serilog;

namespace BaseApp.API.Extentions
{
    public static class WebApplicationExtensions
    {
        public static WebApplication ConfigureMiddlewarePipeline(this WebApplication app)
        {
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
            app.UseRequestLocalization();
            app.UseStaticFiles();
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }

        public static WebApplication ConfigureEndpoints(this WebApplication app)
        {
            app.MapControllers();
            app.UseHangfireDashboard("/hangfire");
            app.MapHub<NotificationHub>("/hubs/notifications");

            return app;
        }
    }

}

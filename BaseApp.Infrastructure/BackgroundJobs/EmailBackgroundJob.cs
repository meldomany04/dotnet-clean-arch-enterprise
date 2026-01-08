using BaseApp.Application.Common.BackgroundJobs;

namespace BaseApp.Infrastructure.BackgroundJobs
{
    public class EmailBackgroundJob : IEmailBackgroundJob
    {
        public Task SendWelcomeEmailAsync(string email)
        {
            Console.WriteLine($"Sending email to {email}");
            return Task.CompletedTask;
        }
    }
}
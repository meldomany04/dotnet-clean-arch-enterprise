namespace BaseApp.Application.Common.BackgroundJobs
{
    public interface IEmailBackgroundJob
    {
        Task SendWelcomeEmailAsync(string email);
    }

}
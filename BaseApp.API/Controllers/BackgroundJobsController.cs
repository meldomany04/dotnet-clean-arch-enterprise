using BaseApp.Application.Common.BackgroundJobs;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace BaseApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BackgroundJobsController : ControllerBase
    {
        private readonly IEmailBackgroundJob _emailJob;

        public BackgroundJobsController(IEmailBackgroundJob emailJob)
        {
            _emailJob = emailJob ?? throw new ArgumentNullException(nameof(emailJob));
        }

        [HttpPost("fire-and-forget")]
        public IActionResult FireAndForget(string email)
        {
            BackgroundJob.Enqueue(() => _emailJob.SendWelcomeEmailAsync(email));
            return Ok("Fire-and-forget job queued");
        }

        [HttpPost("delayed")]
        public IActionResult Delayed(string email, int delayInSeconds = 60)
        {
            BackgroundJob.Schedule(() => _emailJob.SendWelcomeEmailAsync(email),
                                   TimeSpan.FromSeconds(delayInSeconds));
            return Ok($"Delayed job scheduled to run in {delayInSeconds} seconds");
        }

        [HttpPost("recurring")]
        public IActionResult Recurring(string email, string cron = "0 9 * * *") // every day at 9 AM
        {
            RecurringJob.AddOrUpdate(
                $"send-email-{email}",
                () => _emailJob.SendWelcomeEmailAsync(email),
                cron
            );

            return Ok("Recurring job scheduled");
        }

        [HttpPost("continuation")]
        public IActionResult Continuation(string firstEmail, string secondEmail)
        {
            var jobId = BackgroundJob.Enqueue(() => _emailJob.SendWelcomeEmailAsync(firstEmail));

            BackgroundJob.ContinueJobWith(
                jobId,
                () => _emailJob.SendWelcomeEmailAsync(secondEmail)
            );

            return Ok("Continuation job scheduled");
        }
    }
}

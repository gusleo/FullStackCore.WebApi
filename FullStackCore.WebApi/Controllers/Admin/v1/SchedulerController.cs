using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Polly;
using Scaffolding.Auth;
using Scaffolding.Auth.Infrastructure;

namespace FullStackCore.WebApi.Controllers.Admin.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(Policy = MembershipConstant.ArticleEditor)]
    [Route("[area]/v{version:apiVersion}/[controller]")]
    [Area("Admin")]
    public class SchedulerController : BaseController
    {
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IRecurringJobManager _recurringJobManager;
        protected SchedulerController(IAuthenticationService authenticationService, IBackgroundJobClient backgroundJobClient, IRecurringJobManager recurringJobManager) : base(authenticationService)
        {
            _backgroundJobClient = backgroundJobClient;
            _recurringJobManager = recurringJobManager;
        }

        /// <summary>
        /// Create job and execute
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPost("welcome")]
        public IActionResult Welcome(string username)
        {
            var jobId = _backgroundJobClient.Enqueue(() => SendWelcomeMail(username));
            return Ok(jobId);
        }

        /// <summary>
        /// Create cron job and send delayed in 10 mins
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPost("welcome/delayed")]
        public IActionResult DelayedWelcome(string username)
        {
            var jobId = _backgroundJobClient.Schedule(() => SendWelcomeMail(username), TimeSpan.FromMinutes(10));
            return Ok(jobId);
        }

        /// <summary>
        /// Create cron job and run spesific date
        /// </summary>
        /// <param name="username"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        [HttpPost("welcome/schedule")]
        public IActionResult DelayedWelcome(string username, DateTime dateTime)
        {
            // DateTimeOffset of dateTime
            TimeSpan offset = TimeZoneInfo.Local.GetUtcOffset(dateTime);
            DateTimeOffset dateTimeOffset = new(dateTime, offset);

            var jobId = _backgroundJobClient.Schedule(() => SendWelcomeMail(username), dateTimeOffset);
            return Ok(jobId);
        }

        /// <summary>
        /// Create cron job and recurring every week
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPost("recurring")]
        public IActionResult Recurring(string username)
        {
            _recurringJobManager.AddOrUpdate(nameof(SchedulerController.Recurring), () => SendWelcomeMail(username), Cron.Weekly);
            return Ok();
        }

        private void SendWelcomeMail(string username)
        {
            Console.WriteLine($"Send welcome mail for {username}");
        }
    }

}

using Microsoft.AspNetCore.Mvc;
using ClarityEmailer;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Web.Http.Cors;

namespace ClarityTestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SendEmailController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<SendEmailController> _logger;

        public SendEmailController(IConfiguration iConfig, ILogger<SendEmailController> logger)
        {
            _configuration = iConfig;
            _logger = logger;
        }

        [DisableCors]
        [HttpPost(Name = "SendEmail")]
        public EmailResult Post(Email email)
        {
            ClarityEmailer.Emailer emailer = new(
                _configuration.GetValue<string>("Smtp:SmtpServer") ?? "",
                _configuration.GetValue<int>("Smtp:SmtpPort"),
                _configuration.GetValue<string>("Smtp:SmtpCredentials:Username") ?? "",
                _configuration.GetValue<string>("Smtp:SmtpCredentials:Password") ?? "",
                _configuration.GetSection("LogPath").Value ?? ""
            );
            // Discarding here means we can instantly return the response
            _ = emailer.SendEmailAsync(email.From, email.To, email.Subject, email.Body);
            return new EmailResult()
            {
                Success = true
            };
            // As a side note, I have seen some issues with things like AWS Lambda where a Promise
            // was not properly resolved because it wasn't returned to the original function
            // and the main thread exited, causing the whole execution to stop. I imagine some
            // cloud based API setups may still require passing back the task so the server host doesn't
            // end the whole call early
        }
    }
}

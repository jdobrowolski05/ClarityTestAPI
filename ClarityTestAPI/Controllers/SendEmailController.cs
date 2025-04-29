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
        public async Task<EmailResult> Post(Email email)
        {
            ClarityEmailer.Emailer emailer = new (
                _configuration.GetValue<string>("Smtp:SmtpServer") ?? "",
                _configuration.GetValue<int>("Smtp:SmtpPort"),
                _configuration.GetValue<string>("Smtp:SmtpCredentials:Username") ?? "",
                _configuration.GetValue<string>("Smtp:SmtpCredentials:Password") ?? "",
                _configuration.GetSection("LogPath").Value ?? ""
            );
            // Awaiting will cause hang on the response time via API, kind of depends
            // how long the retry delay is, how much info the API needs to send back, etc.
            bool success = await emailer.SendEmailAsync(email.From, email.To, email.Subject, email.Body);
            return new EmailResult() { 
                Success = success
            };
        }
    }
}

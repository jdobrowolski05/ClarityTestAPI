using System.Diagnostics;
using System.Net;
using System.Net.Mail;

namespace ClarityEmailer
{
    public class Emailer
    {
        const int retryMax = 3;
        const int retryDelay = 1000;
        readonly SmtpClient smtpClient;
        readonly string logPath;

        public Emailer(string server, int port, string username, string password, string logPath)
        {
            this.logPath = logPath;
            this.smtpClient = new SmtpClient(server, port)
            {
                UseDefaultCredentials = false,
                EnableSsl = true,
                Credentials = new NetworkCredential(username, password)
            };
        }

        public bool SendEmail(string from, string to, string subject, string body)
        {
            MailMessage message = new(from, to, subject, body);

            return this.InnerSendEmail(message);
       
        }

        public async Task<bool> SendEmailAsync(string from, string to, string subject, string body)
        {
            MailMessage message = new (from, to, subject, body);
            
            return await Task.Run(() =>
            {
                return this.InnerSendEmail(message);
            });
        }

        private bool InnerSendEmail(MailMessage message)
        {
            int tries = 0;
            while (tries < Emailer.retryMax)
            {
                try
                {
                    this.smtpClient.Send(message);

                    // Log success and break out if we didn't throw an exception
                    this.LogEmail(message, true);
                    return true;
                }
                catch (Exception ex)
                {
                    // Log the failure
                    this.LogEmail(message, false, ex);
                    // Increment the try counter and try again after a delay
                    tries++;
                    Thread.Sleep(Emailer.retryDelay);
                }
            }
            // We broke out via excessive tries, therefore failure
            return false;
        }

        private void LogEmail(MailMessage message, bool success, Exception? ex = null)
        {
            // Check if the directory exists and create it if not
            string directory = this.logPath[..logPath.LastIndexOf('/')];
            Directory.CreateDirectory(directory);
            using (StreamWriter logFileWriter = new(this.logPath, append: true))
            {
                logFileWriter.WriteLine("Log Date: {0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
                logFileWriter.WriteLine("Send status: {0}", success ? "Success" : "Failure");
                if(ex != null) { logFileWriter.WriteLine("Failure cause: {0}", ex?.Message); }
                logFileWriter.WriteLine("Sender: {0}, Recipient: {1}", message.From, message.To);
                logFileWriter.WriteLine("Subject: {0}", message.Subject);
                logFileWriter.WriteLine("Body: {0}", message.Body);
            }
        }
    }
}

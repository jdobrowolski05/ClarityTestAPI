using System.Configuration;
using ClarityEmailer;


ClarityEmailer.Emailer emailer = new(
                ConfigurationManager.AppSettings["SmtpServer"] ?? "",
                Int32.Parse(ConfigurationManager.AppSettings["SmtpPort"] ?? "587"),
                ConfigurationManager.AppSettings["SmtpUsername"] ?? "",
                ConfigurationManager.AppSettings["SmtpPassword"] ?? "",
                ConfigurationManager.AppSettings["LogPath"] ?? ""
            );

// So we can send multiple emails and watch the result come back asynchronously
while (true)
{
    Console.WriteLine("Enter 'To' email:");
    string to = Console.ReadLine() ?? "";

    Console.WriteLine("Enter 'From' email:");
    string from = Console.ReadLine() ?? "";

    Console.WriteLine("Enter email subject:");
    string subject = Console.ReadLine() ?? "";

    Console.WriteLine("Enter email body:");
    string body = Console.ReadLine() ?? "";

    // Discard the last task so it's more explicit we are firing and forgetting.
    // Writing the line just gives us a more explicit view of the async return
    // and that we CAN get info about the send if we need it, but it's not blocking us
    Task<bool> result = emailer.SendEmailAsync(from, to, subject, body);
    _ = result.ContinueWith((Task<bool> result) =>
    {
        if (result.Result)
        {
            Console.WriteLine("Email sent successfully");
        } else
        {
            Console.WriteLine("Email failed");
        }
    });
}
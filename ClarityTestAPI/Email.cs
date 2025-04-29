namespace ClarityTestAPI
{
    public class Email
    {
        public required string To { get; set; }

        public required string From { get; set; }

        public required string Subject { get; set; }

        public required string Body { get; set; }

    }

    public class EmailResult
    {
        public required bool Success { get; set; }
    }
}

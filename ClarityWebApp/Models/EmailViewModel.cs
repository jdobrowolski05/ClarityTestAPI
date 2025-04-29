using System.ComponentModel.DataAnnotations;

namespace ClarityWebApp.Models
{
    public class EmailViewModel
    {
        public string? To { get; set; }

        public string? From { get; set; }

        public string? Subject { get; set; }

        [DataType(DataType.MultilineText)]
        public string? Body { get; set; }
    }
}

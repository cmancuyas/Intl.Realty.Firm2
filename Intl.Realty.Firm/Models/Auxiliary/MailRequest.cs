namespace Intl.Realty.Firm.Models.Auxiliary
{
    public class MailRequest
    {
        public string ToEmail { get; set; } = string.Empty;
        public string? CcEmail { get; set; } 
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }

}

namespace Intl.Realty.Firm.Model.ViewModel
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public string ErrorTitle { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
    }
}

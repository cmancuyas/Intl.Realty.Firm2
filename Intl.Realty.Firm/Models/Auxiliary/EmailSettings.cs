namespace Intl.Realty.Firm.Models.Auxiliary
{
    public class EmailSettings
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Host { get; set; }
        public string? DisplayName { get; set; }
        public int Port { get; set; }
    }
}

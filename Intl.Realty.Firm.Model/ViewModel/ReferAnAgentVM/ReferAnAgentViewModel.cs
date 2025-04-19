namespace Intl.Realty.Firm.Model.ViewModel.ReferAnAgentVM
{
    public class ReferAnAgentViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string TelephoneNumber { get; set; } = string.Empty;

        // Optional fields
        public string? ReferredAgentName { get; set; }
        public string? Notes { get; set; }
    }
}

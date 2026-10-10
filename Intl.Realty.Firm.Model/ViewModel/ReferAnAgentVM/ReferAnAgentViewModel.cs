namespace Intl.Realty.Firm.Model.ViewModel.ReferAnAgentVM
{
    public class ReferAnAgentViewModel
    {
        public string Name { get; set; } = string.Empty;

        // Who is being referred
        public string? ReferredAgentName { get; set; }
        public string? ReferredAgentPhone { get; set; }
        public string? Notes { get; set; }
    }
}

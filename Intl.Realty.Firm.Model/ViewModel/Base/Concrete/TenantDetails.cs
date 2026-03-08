using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.Base.Concrete
{
    public class TenantDetails
    {
        [Required(ErrorMessage = "Tenant's name is required.")]
        public string TenantName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tenant's agent name is required.")]
        public string TenantAgentName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tenant's brokerage is required.")]
        public string TenantBrokerage { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tenant's brokerage fax is required.")]
        public string TenantBrokerageFax { get; set; } = string.Empty;

        public string? TenantPhoneNumber { get; set; } = string.Empty;

        public string? TenantEmailAddress { get; set; } = string.Empty;
    }
}

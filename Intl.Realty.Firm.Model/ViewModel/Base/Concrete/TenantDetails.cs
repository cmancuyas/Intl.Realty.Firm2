using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.Base.Concrete
{
    public class TenantDetails
    {
        // Tenant
        [Required(ErrorMessage = "The Tenant's Name field is required.")]
        public string TenantName { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Tenant's Agent Name field is required.")]
        public string TenantAgentName { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Tenant Brokerage field is required.")]
        public string TenantBrokerage { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Tenant Brokerage Fax field is required.")]
        public string TenantBrokerageFax { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Tenant's Phone Number field is required.")]
        public string TenantPhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Tenant's Email Address field is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address format.")]
        public string TenantEmailAddress { get; set; } = string.Empty;
    }
}

using Intl.Realty.Firm.Model.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intl.Realty.Firm.Model.ViewModel.TransactionTypeDetailsBaseVM
{
    public class EditTransactionTypeDetailsBaseLeaseViewModel : BaseModel
    {
        [Required(ErrorMessage = "The Property Address field is required.")]
        public string PropertyAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Final Lease Price field is required.")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal FinalLeasePrice { get; set; }

        [Required(ErrorMessage = "The Final Closing Date field is required.")]
        public DateTime FinalClosingDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "The Deposit Amount field is required.")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal DepositAmount { get; set; }

        [Required(ErrorMessage = "The Deposit Date field is required.")]
        public DateTime DepositDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "The Tenant's Name field is required.")]
        public string TenantName { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Landlord's Name field is required.")]
        public string LandLordName { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Listing Commission Percentage field is required.")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal ListingCommissionPercentage { get; set; }

        [Required(ErrorMessage = "The Buying Commission Percentage field is required.")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal BuyingCommissionPercentage { get; set; }

        [Required(ErrorMessage = "The Listing Agent Name field is required.")]
        public string ListingAgentName { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Listing Brokerage field is required.")]
        public string ListingBrokerage { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Listing Brokerage Fax field is required.")]
        public string ListingBrokerageFax { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Tenant's Agent Name field is required.")]
        public string TenantAgentName { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Tenant Brokerage field is required.")]
        public string TenantBrokerage { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Tenant Brokerage Fax field is required.")]
        public string TenantBrokerageFax { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Landlord's Phone Number field is required.")]
        public string LandLordPhoneNumber { get; set; } = string.Empty;

        //[Required(ErrorMessage = "The Landlord's Lawyer field is required.")]
        //public string LandLordLawyer { get; set; } = string.Empty;
        [Required(ErrorMessage = "The Tenant's Phone Number field is required.")]
        public string TenantPhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Tenant's Email Address field is required.")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "The Tenant's Email Address must be in a valid email format.")]
        public string TenantEmailAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Landlord's Email Address field is required.")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "The Tenant's Email Address must be in a valid email format.")]
        public string LandLordEmailAddress { get; set; } = string.Empty;
        [Required]
        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        [Required]
        public Guid UpdatedBy { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }
    }
}

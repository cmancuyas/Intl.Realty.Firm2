using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Intl.Realty.Firm.Model.ViewModel.Base.Concrete;

namespace Intl.Realty.Firm.Model.ViewModel.TransactionTypeDetailsBaseVM.Base
{
    public class CreateTransactionTypeDetailsBaseLeaseViewModel
    {
        [Required(ErrorMessage = "The Property Address field is required.")]
        public string PropertyAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Final Lease Price field is required.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal FinalLeasePrice { get; set; }

        [Required(ErrorMessage = "The Final Closing Date field is required.")]
        public DateTime FinalClosingDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "The Deposit Amount field is required.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal DepositAmount { get; set; }

        [Required(ErrorMessage = "The Deposit Date field is required.")]
        public DateTime DepositDate { get; set; } = DateTime.Now;

        public TenantDetails TenantDetails{ get; set; } = new TenantDetails();
        public LandLordDetails LandLordDetails { get; set; } = new LandLordDetails();
        public ListingInfo ListingInfo { get; set; } = new ListingInfo();
        // Commission
        public CommissionBase Commission { get; set; } = new CommissionWithListing();

        // Audit
        [Required]
        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        [Required]
        public Guid CreatedBy { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
    }
}

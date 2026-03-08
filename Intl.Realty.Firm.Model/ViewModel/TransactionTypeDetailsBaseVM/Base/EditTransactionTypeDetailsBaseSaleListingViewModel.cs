using Intl.Realty.Firm.Model.ViewModel.Base;
using Intl.Realty.Firm.Model.ViewModel.Base.Concrete;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intl.Realty.Firm.Model.ViewModel.TransactionTypeDetailsBaseVM.Base
{
    public class EditTransactionTypeDetailsBaseSaleListingViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "The Property Address field is required.")]
        public string PropertyAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Final Sale Price field is required.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal FinalSalePrice { get; set; }

        [Required(ErrorMessage = "The Final Closing Date field is required.")]
        public DateTime FinalClosingDate { get; set; }

        [Required(ErrorMessage = "The Deposit Amount field is required.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal DepositAmount { get; set; }

        [Required(ErrorMessage = "The Deposit Date field is required.")]
        public DateTime DepositDate { get; set; }

        public BuyerDetailsWithoutEmail BuyerDetails { get; set; } = new BuyerDetailsWithoutEmail();
        public SellerDetailsWithEmail SellerDetails { get; set; } = new SellerDetailsWithEmail();

        // Commission
        public CommissionBase Commission { get; set; } = new CommissionWithListing();

        // Audit
        [Required]
        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        [Required]
        public Guid UpdatedBy { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.TransactionTypeDetailsBaseVM
{
    public class CreateTransactionTypeDetailsBaseSaleViewModel
    {
        [Required(ErrorMessage = "The Property Address field is required.")]
        public string PropertyAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Final Sale Price field is required.")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal FinalSalePrice { get; set; }

        [Required(ErrorMessage = "The Final Closing Date field is required.")]
        public DateTime FinalClosingDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "The Deposit Amount field is required.")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal DepositAmount { get; set; }

        [Required(ErrorMessage = "The Deposit Date field is required.")]
        public DateTime DepositDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "The Buyer's Name field is required.")]
        public string BuyerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Landlord's Name field is required.")]
        public string LandLordName { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Buying Commission Percentage field is required.")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal BuyingCommissionPercentage { get; set; }

        [Required(ErrorMessage = "The Listing Agent Name field is required.")]
        public string ListingAgentName { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Listing Brokerage field is required.")]
        public string ListingBrokerage { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Listing Brokerage Fax field is required.")]
        public string ListingBrokerageFax { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Buyer's Agent Name field is required.")]
        public string BuyerAgentName { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Buyer Brokerage field is required.")]
        public string BuyerBrokerage { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Buyer Brokerage Fax field is required.")]
        public string BuyerBrokerageFax { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Seller's Lawyer field is required.")]
        public string SellersLawyer { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Seller's Lawyer Address field is required.")]
        public string SellersLawyerAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Seller's Phone Number field is required.")]
        public string SellersPhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Buyer's Lawyer field is required.")]
        public string BuyersLawyer { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Buyer's Lawyer Address field is required.")]
        public string BuyersLawyerAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Buyer's Phone Number field is required.")]
        public string BuyersPhoneNumber { get; set; } = string.Empty;

        [Required]
        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        [Required]
        public Guid CreatedBy { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
    }
}

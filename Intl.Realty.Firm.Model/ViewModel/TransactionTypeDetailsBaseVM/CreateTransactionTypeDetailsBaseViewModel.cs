using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.TransactionTypeDetailsBaseVM
{
    public class CreateTransactionTypeDetailsBaseViewModel
    {
        [Required(ErrorMessage ="Property Address field is required")]
        public string PropertyAddress { get; set; } = string.Empty;
        [Required(ErrorMessage = "Final Sale Price field is required")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal FinalSalePrice { get; set; }
        [Required(ErrorMessage = "Final Closing Date field is required")]
        public DateTime FinalClosingDate { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "Deposit Amount field is required")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal DepositAmount { get; set; }
        [Required(ErrorMessage = "Deposit Date field is required")]
        public DateTime DepositDate { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "Buyers Name field is required")]
        public string BuyerName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Land Lords field is required")]
        public string LandLordName { get; set; } = string.Empty;
        [Column(TypeName = "decimal(18,4)")]
        [Required(ErrorMessage = "Listing Commission Percentage field is required")]
        public decimal ListingCommissionPercentage { get; set; }
        [Required(ErrorMessage = "Buying Commission Percentage field is required")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal BuyingCommissionPercentage { get; set; }
        [Required(ErrorMessage = "Listing Agent Name field is required")]
        public string ListingAgentName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Listing Brokerage field is required")]
        public string ListingBrokerage { get; set; } = string.Empty;
        [Required(ErrorMessage = "Listing Brokerage Fax field is required")]
        public string ListingBrokerageFax { get; set; } = string.Empty;
        [Required(ErrorMessage = "Buyer Agent Name field is required")]
        public string BuyerAgentName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Buyer Brokerage field is required")]
        public string BuyerBrokerage { get; set; } = string.Empty;
        [Required(ErrorMessage = "Buyer Brokerage Fax field is required")]
        public string BuyerBrokerageFax { get; set; } = string.Empty;
        [Required(ErrorMessage = "Sellers Lawyer field is required")]
        public string SellersLawyer { get; set; } = string.Empty;
        [Required(ErrorMessage = "Sellers Lawyers Address field is required")]
        public string SellersLawyerAddress { get; set; } = string.Empty;
        [Required(ErrorMessage = "Sellers Phone Number field is required")]
        public string SellersPhoneNumber { get; set; } = string.Empty;
        [Required(ErrorMessage = "Buyers Lawyer field is required")]
        public string BuyersLawyer { get; set; } = string.Empty;
        [Required(ErrorMessage = "Buyers Lawyer Address field is required")]
        public string BuyersLawyerAddress { get; set; } = string.Empty;
        [Required(ErrorMessage = "Buyers Phone Number field is required")]
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

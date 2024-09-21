using System.ComponentModel.DataAnnotations.Schema;

namespace Intl.Realty.Firm.Model.ViewModel.TransactionTypeDetailsBaseVM
{
    public class EditTransactionTypeDetailsBaseViewModel
    {
        public string PropertyAddress { get; set; } = string.Empty;
        [Column(TypeName = "decimal(18,4)")]
        public Decimal FinalSalePrice { get; set; }
        public DateTime FinalClosingDate { get; set; } = DateTime.UtcNow;
        [Column(TypeName = "decimal(18,4)")]
        public Decimal DepositAmount { get; set; }
        public DateTime DepositDate { get; set; } = DateTime.UtcNow;
        public string BuyerName { get; set; } = string.Empty;
        public string LandLordName { get; set; } = string.Empty;
        [Column(TypeName = "decimal(18,4)")]
        public Decimal ListingCommissionPercentage { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public Decimal BuyingCommissionPercentage { get; set; }
        public string ListingAgentName { get; set; } = string.Empty;
        public string ListingBrokerage { get; set; } = string.Empty;
        public string ListingBrokerageFax { get; set; } = string.Empty;
        public string BuyerAgentName { get; set; } = string.Empty;
        public string BuyerBrokerage { get; set; } = string.Empty;
        public string BuyerBrokerageFax { get; set; } = string.Empty;
        public string SellersLawyer { get; set; } = string.Empty;
        public string SellersLawyerAddress { get; set; } = string.Empty;
        public string SellersPhoneNumber { get; set; } = string.Empty;
        public string BuyersLawyer { get; set; } = string.Empty;
        public string BuyersLawyerAddress { get; set; } = string.Empty;
        public string BuyersPhoneNumber { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace Intl.Realty.Firm.Model.Model
{
    public class TransactionTypeDetailsBase : BaseModel
    {
        public string PropertyAddress { get; set; } = string.Empty;
        [Column(TypeName = "decimal(18,4)")]
        public decimal? FinalSalePrice { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? FinalLeasePrice { get; set; }
        public DateTime FinalClosingDate { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal DepositAmount { get; set; }
        public DateTime DepositDate { get; set; }
        public string BuyerName { get; set; } = string.Empty;
        public string LandLordName { get; set; } = string.Empty;
        [Column(TypeName = "decimal(18,4)")]
        public decimal ListingCommissionPercentage { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal BuyingCommissionPercentage { get; set; }
        public string? ListingAgentName { get; set; } = string.Empty;
        public string? ListingBrokerage { get; set; } = string.Empty;
        public string? ListingBrokerageFax { get; set; } = string.Empty;
        public string? BuyerAgentName { get; set; } = string.Empty;
        public string? BuyerBrokerage { get; set; } = string.Empty;
        public string? BuyerBrokerageFax { get; set; } = string.Empty;
        public string? SellersLawyer { get; set; } = string.Empty;
        public string? SellersLawyerAddress { get; set; } = string.Empty;
        public string? SellersPhoneNumber { get; set; } = string.Empty;
        public string? BuyersLawyer { get; set; } = string.Empty;
        public string? BuyersLawyerAddress { get; set; } = string.Empty;
        public string? BuyersPhoneNumber { get; set; } = string.Empty;
        public string? TenantName { get; set; } = string.Empty;
        public string? TenantAgentName { get; set; } = string.Empty;
        public string? TenantBrokerage { get; set; } = string.Empty;
        public string? TenantBrokerageFax { get; set; } = string.Empty;
        public string? TenantPhoneNumber { get; set; } = string.Empty;
        public string? TenantEmailAddress { get; set; } = string.Empty;
        public string? LandLordPhoneNumber { get; set; } = string.Empty;
        public string? LandLordEmailAddress { get; set; } = string.Empty;
        
    }
}

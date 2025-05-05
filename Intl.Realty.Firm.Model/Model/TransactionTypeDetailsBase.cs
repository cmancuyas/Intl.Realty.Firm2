using System.ComponentModel.DataAnnotations.Schema;

namespace Intl.Realty.Firm.Model.Model
{
    public class TransactionTypeDetailsBase : BaseModel
    {
        public string PropertyAddress { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal? FinalSalePrice { get; set; }

        public DateTime FinalClosingDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DepositAmount { get; set; }

        public DateTime DepositDate { get; set; }

        public string BuyerName { get; set; } = string.Empty;
        public string LandLordName { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal ListingCommissionPercentage { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal BuyingCommissionPercentage { get; set; }

        public string? ListingAgentName { get; set; }
        public string? ListingBrokerage { get; set; }
        public string? ListingBrokerageFax { get; set; }

        public string? BuyerLawyer { get; set; }
        public string? BuyerLawyerAddress { get; set; }
        public string? BuyerEmail { get; set; } = string.Empty;

        public string? BuyerAgentName { get; set; }
        public string? BuyerBrokerage { get; set; }
        public string? BuyerBrokerageFax { get; set; }
        public string? BuyerBrokeragePhoneNumber { get; set; }
        public string? BuyerBrokerageEmail { get; set; }

        public string? SellerName { get; set; }
        public string? SellerAgentName { get; set; }
        public string? SellerLawyer { get; set; }
        public string? SellerLawyerAddress { get; set; }
        public string? SellerEmail { get; set; }
        public string? SellerBrokerage { get; set; }
        public string? SellerBrokerageFax { get; set; }
        public string? SellerBrokeragePhoneNumber { get; set; }
        public string? SellerBrokerageEmail { get; set; }

        public string? TenantName { get; set; }
        public string? TenantAgentName { get; set; }
        public string? TenantBrokerage { get; set; }
        public string? TenantBrokerageFax { get; set; }

        public string? LandLordPhoneNumber { get; set; }
        public string? LandLordEmailAddress { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? FinalLeasePrice { get; set; }

        public string? TenantEmailAddress { get; set; }
        public string? TenantPhoneNumber { get; set; }

        public string? SellerLawyerPhone { get; set; }
        public string? SellerLawyerEmail { get; set; }
        public string? BuyerLawyerPhone { get; set; }
        public string? BuyerLawyerEmail { get; set; }

    }
}

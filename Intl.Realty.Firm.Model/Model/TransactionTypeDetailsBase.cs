using System.ComponentModel.DataAnnotations.Schema;

namespace Intl.Realty.Firm.Model.Model
{
    public class TransactionTypeDetailsBase : BaseModel
    {
        public string PropertyAddress { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,4)")]
        public decimal? FinalSalePrice { get; set; }

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

        public string? ListingAgentName { get; set; }
        public string? ListingBrokerage { get; set; }
        public string? ListingBrokerageFax { get; set; }

        public string? BuyerAgentName { get; set; }
        public string? BuyerBrokerage { get; set; }
        public string? BuyerBrokerageFax { get; set; }

        public string? SellersLawyer { get; set; }
        public string? SellersLawyerAddress { get; set; }
        public string? SellersPhoneNumber { get; set; }

        public string? BuyersLawyer { get; set; }
        public string? BuyersLawyerAddress { get; set; }
        public string BuyersPhoneNumber { get; set; } = string.Empty;

        public string? TenantName { get; set; }
        public string? TenantAgentName { get; set; }
        public string? TenantBrokerage { get; set; }
        public string? TenantBrokerageFax { get; set; }

        public string? LandLordPhoneNumber { get; set; }
        public string? LandLordEmailAddress { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal? FinalLeasePrice { get; set; }

        public string? TenantEmailAddress { get; set; }
        public string? TenantPhoneNumber { get; set; }

        public string? SellerLawyerPhone { get; set; }
        public string? SellerLawyerEmail { get; set; }
        public string? BuyerLawyerPhone { get; set; }
        public string? BuyerLawyerEmail { get; set; }

    }
}

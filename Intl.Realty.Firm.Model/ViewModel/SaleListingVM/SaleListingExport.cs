using Intl.Realty.Firm.Model.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intl.Realty.Firm.Model.ViewModel.SaleListingVM
{
    public class SaleListingExport
    {
        [Required]
        public string DealTransactionNumber { get; set; } = string.Empty;
        public string TransactionType { get; set; } = string.Empty;
        public string DocumentType { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }
        public DateTime DateSubmitted { get; set; }
        public string PropertyAddress { get; set; } = string.Empty;
        [Column(TypeName = "decimal(18,2)")]
        public decimal FinalSalePrice { get; set; }
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
        public string ListingAgentName { get; set; } = string.Empty;
        public string ListingBrokerage { get; set; } = string.Empty;
        public string ListingBrokerageFax { get; set; } = string.Empty;
        public string BuyerAgentName { get; set; } = string.Empty;
        public string BuyerBrokerage { get; set; } = string.Empty;
        public string BuyerBrokerageFax { get; set; } = string.Empty;
        public string SellerLawyer { get; set; } = string.Empty;
        public string SellerLawyerAddress { get; set; } = string.Empty;
        public string SellerEmailPhone { get; set; } = string.Empty;
        public string BuyerLawyer { get; set; } = string.Empty;
        public string BuyerLawyerAddress { get; set; } = string.Empty;
        public string BuyerEmailPhone { get; set; } = string.Empty;
        public List<FileUpload>? FileUploads { get; set; }
        public int DealStatusId { get; set; }
        public string DealStatus { get; set; } = string.Empty;
        public string CreatedByFullName { get; set; } = string.Empty;
        public string? UpdatedByFullName { get; set; }
        
    }
}

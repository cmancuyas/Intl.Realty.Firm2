using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.AllDealsVM
{
    public class AllDealViewModel
    {
        public int Id { get; set; }
        public int TransactionTypeId { get; set; }
        public int BridgeId { get; set; }
        [Required]
        public string DealTransactionNumber { get; set; } = string.Empty;
        public string TransactionTypeName { get; set; } = string.Empty;
        public string PropertyAddress { get; set; } = string.Empty;
        public string DealStatusName { get; set; } = string.Empty;
        public List<string>? FileExtensions { get; set; }
        public DateTime FinalClosingDate { get; set; }
        public string CreatedByFullName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime SubmittedAt { get; set; }
        public string UpdatedByFullName { get; set; } = string.Empty;
        public DateTime UpdatedAt { get; set; }
        public string DeletedByFullName { get; set; } = string.Empty;
        public DateTime DeletedAt { get; set; }
    }
}

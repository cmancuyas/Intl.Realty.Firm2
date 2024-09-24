using Intl.Realty.Firm.Model.Model;

namespace Intl.Realty.Firm.Model.ViewModel.SaleListingVM
{
    public class SaleListingViewModel : BaseModel
    {
        public int Id { get; set; }
        public int TransactionTypeId { get; set; }
        public int IRFDealId { get; set; }
        public int DealStatusId { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public List<FileUpload>? FileUploads { get; set; }
        public string? Notes { get; set; }
        public IRFDeal? IRFDeal { get; set; }
        public TransactionType? TransactionType { get; set; }
        public DealStatus? DealStatus { get; set; }
        public string? CreatedByFullName { get; set; }
        public string? UpdatedByFullName { get; set; }
        public string? DeletedByFullName { get; set; }
    }
}

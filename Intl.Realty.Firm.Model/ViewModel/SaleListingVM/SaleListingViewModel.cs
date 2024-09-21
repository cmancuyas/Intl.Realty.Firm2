using Intl.Realty.Firm.Model.Model;
using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.SaleListingVM
{
    public class SaleListingViewModel : BaseModel
    {
        [Key]
        public int Id { get; set; }
        public int TransactionTypeId { get; set; }
        public int IRFDealId { get; set; }
        public List<FileUpload>? FileUploads { get; set; }
        public int DealStatusId { get; set; }
        public DealStatus? DealStatus { get; set; }
        public string? CreatedByFullName { get; set; }
        public string? UpdatedByFullName { get; set; }
        public TransactionType? TransactionType { get; set; } 
        public DocumentType? DocumentType { get; set; } 
        public IRFDeal? IRFDeal { get; set; }
    }
}

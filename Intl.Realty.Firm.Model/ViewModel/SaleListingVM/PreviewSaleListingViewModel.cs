using Intl.Realty.Firm.Model.Model;
using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.SaleListingVM
{
    public class PreviewSaleListingViewModel : BaseModel
    {
        [Key]
        public int Id { get; set; }
        public int TransactionTypeId { get; set; }
        public TransactionType? TransactionType { get; set; } = new TransactionType();
        public int IRFDealId { get; set; }
        public IRFDeal? IRFDeal { get; set; } = new IRFDeal();
        public List<FileUpload>? FileUploads { get; set; }
        public int DealStatusId { get; set; }
        public DealStatus? DealStatus { get; set; }
    }
}

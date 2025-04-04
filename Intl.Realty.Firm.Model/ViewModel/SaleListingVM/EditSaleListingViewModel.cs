using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.ViewModel.FileUploadVM;
using Intl.Realty.Firm.Model.ViewModel.IRFDealVM;
using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.SaleListingVM
{
    public class EditSaleListingViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string DealTransactionNumber { get; set; } = string.Empty;
        public int TransactionTypeId { get; set; }
        public int IRFDealId { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public EditIRFDealSaleListingViewModel EditIRFDealSaleListingViewModel { get; set; } = new EditIRFDealSaleListingViewModel();
        public CreateFileUploadListViewModel? CreateFileUploadListViewModel { get; set; }
        public List<FileUpload>? FileUploads { get; set; }
        public string? Notes { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int DealStatusId { get; set; }
        public TransactionType? TransactionType { get; set; }
        public DealStatus? DealStatus { get; set; }
        public IEnumerable<DocumentType>? DocumentTypeList { get; set; }
        public IEnumerable<DealStatus>? DealStatusList { get; set; }
        public string UpdatedByFullName { get; set; } = string.Empty;
    }
}

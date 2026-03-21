using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.ViewModel.AdditionalDocumentVM;
using Intl.Realty.Firm.Model.ViewModel.FileUploadVM;
using Intl.Realty.Firm.Model.ViewModel.IRFDealVM;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.SaleListingVM
{
    public class CreateSaleListingViewModel
    {
        [Required]
        public string DealTransactionNumber { get; set; } = string.Empty;
        public int TransactionTypeId { get; set; }
        public int IRFDealId { get; set; }
        public CreateIRFDealSaleListingViewModel CreateIRFDealSaleListingViewModel { get; set; } = new CreateIRFDealSaleListingViewModel();
        public CreateFileUploadListViewModel CreateFileUploadListViewModel { get; set; } = new CreateFileUploadListViewModel();
        public List<AdditionalDocumentInputViewModel> AdditionalDocuments { get; set; } = new();
        public int DealStatusId { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public string? Notes { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedByFullName { get; set; } = string.Empty;
        public TransactionType? TransactionType { get; set; }
        public DealStatus? DealStatus { get; set; }
        public IEnumerable<DocumentType>? DocumentTypeList { get; set; }
        public IEnumerable<DocumentTypeAssignment>? DocumentTypeAssignmentList { get; set; }
        public IEnumerable<SelectListItem>? SignageIEnum { get; set; }
        public int DefaultSignage { get; set; }
        // 🔒 Add this for concurrency control
        [Timestamp]
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    }
}

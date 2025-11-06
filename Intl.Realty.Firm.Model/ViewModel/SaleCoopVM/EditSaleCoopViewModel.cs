using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.ViewModel.FileUploadVM;
using Intl.Realty.Firm.Model.ViewModel.IRFDealVM;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.SaleCoopVM
{
    public class EditSaleCoopViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string DealTransactionNumber { get; set; } = string.Empty;
        public int TransactionTypeId { get; set; }
        public int IRFDealId { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public EditIRFDealSaleCoopViewModel EditIRFDealSaleCoopViewModel { get; set; } = new EditIRFDealSaleCoopViewModel();
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
        public List<DocumentType>? DocumentTypeList { get; set; }
        public IEnumerable<DocumentTypeAssignment>? DocumentTypeAssignmentList { get; set; }
        public IEnumerable<DealStatus>? DealStatusList { get; set; }
        public string UpdatedByFullName { get; set; } = string.Empty;

        public IEnumerable<SelectListItem>? SignageIEnum { get; set; }
        public int DefaultSignage { get; set; }
        // 🔒 Add this for concurrency control
        [Timestamp]
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    }
}

using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.ViewModel.FileUploadVM;
using Intl.Realty.Firm.Model.ViewModel.IRFDealVM;

namespace Intl.Realty.Firm.Model.ViewModel.LeaseCoopVM
{
    public class CreateLeaseCoopViewModel
    {
        public string DealTransactionNumber { get; set; } = string.Empty;
        public int TransactionTypeId { get; set; }
        public int IRFDealId { get; set; }
        public CreateIRFDealLeaseCoopViewModel CreateIRFDealLeaseCoopViewModel { get; set; } = new CreateIRFDealLeaseCoopViewModel();
        public CreateFileUploadListViewModel CreateFileUploadListViewModel { get; set; } = new CreateFileUploadListViewModel();
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
    }
}

namespace Intl.Realty.Firm.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IDepartmentRepository Department { get; }
        IEmploymentStatusRepository EmploymentStatus { get; }
        ITransactionTypeRepository TransactionType { get; }
        IDocumentTypeRepository DocumentType { get; }
        IDealStatusRepository DealStatus { get; }
        IDocumentTypeAssignmentRepository DocumentTypeAssignment { get; }
        IIRFDealRepository IRFDeal { get; }
        IFileUploadRepository FileUpload { get; }
        ISaleListingRepository SaleListing { get; }
        ISaleCoopRepository SaleCoop { get; }
        ILeaseListingRepository LeaseListing { get; }
        ILeaseCoopRepository LeaseCoop { get; }
        void SaveAsync();
        void Save();
    }
}

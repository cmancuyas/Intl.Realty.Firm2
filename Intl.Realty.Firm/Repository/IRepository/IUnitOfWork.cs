namespace Intl.Realty.Firm.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IDealTransactionNumberRangeRepository DealTransactionNumberRange { get; }
        IInternalEmailRepository InternalEmail { get; }
        IDepartmentRepository Department { get; }
        IProvinceRepository Province { get; }
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
        ICustomerRepository Customer { get; }
        IEmailCategoryRepository EmailCategory { get; }
        IContactEmailRepository ContactEmail { get; }
        Task SaveChangesAsync();
        void SaveChanges();
    }
}

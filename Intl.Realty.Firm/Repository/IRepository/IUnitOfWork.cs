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
        void SaveAsync();
        void Save();
    }
}

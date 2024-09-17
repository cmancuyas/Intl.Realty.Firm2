using Intl.Realty.Firm.DataAccess;
using Intl.Realty.Firm.Repository.IRepository;

namespace Intl.Realty.Firm.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public IDepartmentRepository Department { get; private set; }
        public IEmploymentStatusRepository EmploymentStatus { get; private set; }
        public ITransactionTypeRepository TransactionType { get; private set; }
        public IDocumentTypeRepository DocumentType { get; private set; }
        public IDealStatusRepository DealStatus { get; private set; }
        public IDocumentTypeAssignmentRepository DocumentTypeAssignment { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Department = new DepartmentRepository(_db);
            EmploymentStatus = new EmploymentStatusRepository(_db);
            TransactionType = new TransactionTypeRepository(_db);
            DocumentType = new DocumentTypeRepository(_db);
            DealStatus = new DealStatusRepository(_db);
            DocumentTypeAssignment = new DocumentTypeAssignmentRepository(_db);
        }
        public void SaveAsync()
        {
            _db.SaveChangesAsync();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}

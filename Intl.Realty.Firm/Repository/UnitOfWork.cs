using Intl.Realty.Firm.DataAccess;
using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Repository.IRepository;

namespace Intl.Realty.Firm.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public IDepartmentRepository Department { get; private set; }
        public IEmploymentStatusRepository EmploymentStatus { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Department = new DepartmentRepository(_db);
            EmploymentStatus = new EmploymentStatusRepository(_db);
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

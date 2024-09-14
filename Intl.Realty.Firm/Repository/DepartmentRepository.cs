using Intl.Realty.Firm.DataAccess;
using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Repository.IRepository;

namespace Intl.Realty.Firm.Repository
{
    public class DepartmentRepository : Repository<Department>, IDepartmentRepository
    {
        private ApplicationDbContext _db;
        public DepartmentRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}

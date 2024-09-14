using Intl.Realty.Firm.DataAccess;
using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Repository.IRepository;

namespace Intl.Realty.Firm.Repository
{
    public class EmploymentStatusRepository : Repository<EmploymentStatus>, IEmploymentStatusRepository
    {
        private ApplicationDbContext _db;
        public EmploymentStatusRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}

using Intl.Realty.Firm.DataAccess;
using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Repository.IRepository;

namespace Intl.Realty.Firm.Repository
{
    public class LeaseCoopRepository : Repository<LeaseCoop>, ILeaseCoopRepository
    {
        private ApplicationDbContext _db;
        public LeaseCoopRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}

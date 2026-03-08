using Intl.Realty.Firm.DataAccess;
using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Repository.IRepository;

namespace Intl.Realty.Firm.Repository
{
    public class LeaseListingRepository : Repository<LeaseListing>, ILeaseListingRepository
    {
        private ApplicationDbContext _db;
        public LeaseListingRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}

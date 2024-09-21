using Intl.Realty.Firm.DataAccess;
using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Repository.IRepository;

namespace Intl.Realty.Firm.Repository
{
    public class SaleListingRepository : Repository<SaleListing>, ISaleListingRepository
    {
        private ApplicationDbContext _db;
        public SaleListingRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}

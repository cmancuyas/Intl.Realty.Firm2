using Intl.Realty.Firm.DataAccess;
using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Repository.IRepository;

namespace Intl.Realty.Firm.Repository
{
    public class SaleCoopRepository : Repository<SaleCoop>, ISaleCoopRepository
    {
        private ApplicationDbContext _db;
        public SaleCoopRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}

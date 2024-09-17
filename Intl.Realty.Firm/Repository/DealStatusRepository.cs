using Intl.Realty.Firm.DataAccess;
using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Repository.IRepository;

namespace Intl.Realty.Firm.Repository
{
    public class DealStatusRepository : Repository<DealStatus>, IDealStatusRepository
    {
        private ApplicationDbContext _db;
        public DealStatusRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}

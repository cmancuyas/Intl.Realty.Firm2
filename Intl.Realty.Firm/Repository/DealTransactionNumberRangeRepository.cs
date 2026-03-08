using Intl.Realty.Firm.DataAccess;
using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Repository.IRepository;

namespace Intl.Realty.Firm.Repository
{
    public class DealTransactionNumberRangeRepository : Repository<DealTransactionNumberRange>, IDealTransactionNumberRangeRepository
    {
        private ApplicationDbContext _db;
        public DealTransactionNumberRangeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}

using Intl.Realty.Firm.DataAccess;
using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Repository.IRepository;

namespace Intl.Realty.Firm.Repository
{
    public class IRFDealRepository : Repository<IRFDeal>, IIRFDealRepository
    {
        private ApplicationDbContext _db;
        public IRFDealRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}

using Intl.Realty.Firm.DataAccess;
using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Repository.IRepository;

namespace Intl.Realty.Firm.Repository
{
    public class InternalEmailRepository : Repository<InternalEmail>, IInternalEmailRepository
    {
        private ApplicationDbContext _db;
        public InternalEmailRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}

using Intl.Realty.Firm.DataAccess;
using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Repository.IRepository;

namespace Intl.Realty.Firm.Repository
{
    public class ContactEmailRepository : Repository<ContactEmail>, IContactEmailRepository
    {
        private ApplicationDbContext _db;
        public ContactEmailRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}

using Intl.Realty.Firm.DataAccess;
using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Repository.IRepository;

namespace Intl.Realty.Firm.Repository
{
    public class EmailStatusRepository : Repository<EmailStatus>, IEmailStatusRepository
    {
        private ApplicationDbContext _db;
        public EmailStatusRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}

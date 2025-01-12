using Intl.Realty.Firm.DataAccess;
using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Repository.IRepository;

namespace Intl.Realty.Firm.Repository
{
    public class EmailCategoryRepository : Repository<EmailCategory>, IEmailCategoryRepository
    {
        private ApplicationDbContext _db;
        public EmailCategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}

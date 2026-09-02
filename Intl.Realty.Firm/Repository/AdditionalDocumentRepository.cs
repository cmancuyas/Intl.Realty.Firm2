using Intl.Realty.Firm.DataAccess;
using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Repository.IRepository;

namespace Intl.Realty.Firm.Repository
{
    public class AdditionalDocumentRepository : Repository<AdditionalDocument>, IAdditionalDocumentRepository
    {
        private ApplicationDbContext _db;
        public AdditionalDocumentRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}

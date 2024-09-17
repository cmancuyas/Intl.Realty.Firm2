using Intl.Realty.Firm.DataAccess;
using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Repository.IRepository;

namespace Intl.Realty.Firm.Repository
{
    public class DocumentTypeAssignmentRepository : Repository<DocumentTypeAssignment>, IDocumentTypeAssignmentRepository
    {
        private ApplicationDbContext _db;
        public DocumentTypeAssignmentRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}

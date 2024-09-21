using Intl.Realty.Firm.DataAccess;
using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Repository.IRepository;

namespace Intl.Realty.Firm.Repository
{
    public class FileUploadRepository : Repository<FileUpload>, IFileUploadRepository
    {
        private ApplicationDbContext _db;
        public FileUploadRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}

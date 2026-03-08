using Intl.Realty.Firm.DataAccess;
using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Models;
using Intl.Realty.Firm.Repository.IRepository;
using System.Linq.Expressions;

namespace Intl.Realty.Firm.Repository
{
    public class SaleListingRepository : Repository<SaleListing>, ISaleListingRepository
    {
        private ApplicationDbContext _db;
        public SaleListingRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}

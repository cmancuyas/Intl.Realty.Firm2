using Intl.Realty.Firm.DataAccess;
using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Repository.IRepository;

namespace Intl.Realty.Firm.Repository
{
    public class AgentReferralRepository : Repository<AgentReferral>, IAgentReferralRepository
    {
        private ApplicationDbContext _db;
        public AgentReferralRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}

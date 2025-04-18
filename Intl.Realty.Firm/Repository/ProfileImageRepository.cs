using Intl.Realty.Firm.DataAccess;
using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Intl.Realty.Firm.Repository
{
    public class ProfileImageRepository : Repository<ProfileImage>, IProfileImageRepository
    {
        private ApplicationDbContext _db;
        public ProfileImageRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<ProfileImage?> GetByUserIdAsync(Guid userId)
        {
            return await _db.ProfileImages
                .Join(
                    _db.Users,
                    image => image.Id,
                    user => user.ProfileImageId ?? 0,
                    (image, user) => new { Image = image, User = user }
                )
                .Where(joined => joined.User.Id == userId)
                .Select(joined => joined.Image)
                .FirstOrDefaultAsync();
        }

    }
}

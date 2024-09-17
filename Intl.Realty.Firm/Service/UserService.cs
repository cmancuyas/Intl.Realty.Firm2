using Intl.Realty.Firm.DataAccess;
using Intl.Realty.Firm.Model.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Intl.Realty.Firm.Service
{
    public class UserService : UserManager<ApplicationUser>
    {
        private readonly ApplicationDbContext context;

        public UserService(ApplicationDbContext context, IUserStore<ApplicationUser> store, 
            IOptions<IdentityOptions> optionsAccessor, 
            IPasswordHasher<ApplicationUser> passwordHasher, 
            IEnumerable<IUserValidator<ApplicationUser>> userValidators, 
            IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators, 
            ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, 
            IServiceProvider services, ILogger<UserManager<ApplicationUser>> logger) : 
                        base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            this.context = context;
        }

        public override async Task<ApplicationUser?> GetUserAsync(ClaimsPrincipal principal)
        {
            var user = await base.GetUserAsync(principal);

            if (user is null)
                return user;

            await context.Entry(user)
                .Reference(x => x.Department)
                .LoadAsync();

            return user;
        }
    }
}

using Microsoft.AspNetCore.Identity;

namespace Intl.Realty.Firm.Model.Model
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public ApplicationRole()
        {
            
        }
        public ApplicationRole(string roleName) : this()
        {
            Name = roleName;
        }
    }
}

using Microsoft.AspNetCore.Identity;

namespace Intl.Realty.Firm.DataAccess.Model
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public int DepartmentId { get; set; }
        public int EmploymentStatusId { get; set; }
        public int ProfilePictureId { get; set; }
    }
}

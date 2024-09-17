using Microsoft.AspNetCore.Identity;

namespace Intl.Realty.Firm.Model.Model
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Suffix { get; set; }
        public int? DepartmentId { get; set; }
        public int EmploymentStatusId { get; set; }
        public int? ProfilePictureId { get; set; }
        public bool UserAgreement { get; set; }

        public virtual Department? Department { get; set; }
        public virtual EmploymentStatus? EmploymentStatus { get; set; }

    }
}

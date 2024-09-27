using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intl.Realty.Firm.Model.Model
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        [ForeignKey("Department")]
        public int? DepartmentId { get; set; }
        [ForeignKey("EmploymentStatus")]
        public int EmploymentStatusId { get; set; }
        public int? ProfilePictureId { get; set; }
        public bool UserAgreement { get; set; }
        public DateTime RegistrationDate { get; set; }
        public virtual Department? Department { get; set; }
        public virtual EmploymentStatus? EmploymentStatus { get; set; }

    }
}

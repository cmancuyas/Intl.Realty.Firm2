using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intl.Realty.Firm.Model.Model
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [ForeignKey("Department")]
        public int? DepartmentId { get; set; }

        [ForeignKey("EmploymentStatus")]
        public int EmploymentStatusId { get; set; }
        [ForeignKey("EmailStatus")]
        public int EmailStatusId { get; set; }

        public bool UserAgreement { get; set; }
        public DateTime RegistrationDate { get; set; }
        public virtual Department? Department { get; set; }
        public virtual EmploymentStatus? EmploymentStatus { get; set; }
        public virtual EmailStatus? EmailStatus { get; set; }

        // 🔹 Self-referencing foreign key to link an Agent to a TeamLeader
        [ForeignKey("TeamLeader")]
        public Guid? TeamLeaderId { get; set; } // Nullable in case the user is not an Agent
        public virtual ApplicationUser? TeamLeader { get; set; } // Navigation property

        public virtual ICollection<ApplicationUser>? Agents { get; set; } // TeamLeader's Agents

        public int? ProfileImageId { get; set; }

        [ForeignKey("ProfileImageId")]
        public virtual ProfileImage? ProfileImage { get; set; }


    }

}

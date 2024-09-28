using Intl.Realty.Firm.Model.Model;
using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.UserVM
{
    public class EditUserViewModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int? DepartmentId { get; set; }
        public int EmploymentStatusId { get; set; }
        public bool EmailConfirmed { get; set; }
        public int? ProfilePictureId { get; set; }
        public List<string> Claims { get; set; } = new List<string>();
        public List<string> Roles { get; set; } = new List<string>();
        public List<Department>? DepartmentList { get; set; }
        public List<EmploymentStatus>? EmploymentStatusList { get; set; }
    }
}

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
        public string? MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Suffix { get; set; }
        public int? DepartmentId { get; set; }
        public int EmploymentStatusId { get; set; }
        public int? ProfilePictureId { get; set; }
        public List<string> Claims { get; set; } = new List<string>();
        public List<string> Roles { get; set; } = new List<string>();
    }
}

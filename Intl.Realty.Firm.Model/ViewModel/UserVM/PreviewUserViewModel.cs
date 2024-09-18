using Intl.Realty.Firm.Model.Model;

namespace Intl.Realty.Firm.Model.ViewModel.UserVM
{
    public class PreviewUserViewModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public string EmploymentStatusName { get; set; } = string.Empty;
        public int? ProfilePictureId { get; set; }
        public List<string>? Roles { get; set; }
    }

}

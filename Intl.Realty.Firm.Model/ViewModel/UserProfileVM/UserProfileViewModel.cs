namespace Intl.Realty.Firm.Model.ViewModel.UserProfileVM
{
    public class UserProfileViewModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? DepartmentName { get; set; }
        public string EmploymentStatusName { get; set; } = string.Empty;
        public int? ProfilePictureId { get; set; }
        public bool EmailConfirmed { get; set; }
        public List<string> Claims { get; set; } = new List<string>();
        public List<string> Roles { get; set; } = new List<string>();
    }
}

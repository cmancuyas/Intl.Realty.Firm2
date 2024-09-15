namespace Intl.Realty.Firm.Model.ViewModel.AccountVM
{
    public class UserRolesViewModel
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;

        public bool IsSelected { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.RoleVM
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            Users = new List<string>();
        }
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Role Name is required")]
        public string RoleName { get; set; } = string.Empty;

        public List<string> Users { get; set; }
    }
}

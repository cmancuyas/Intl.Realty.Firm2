using Intl.Realty.Firm.Model.Model;
using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.RoleVM
{
    public class PreviewRoleViewModel
    {
        public PreviewRoleViewModel()
        {
            Users = new List<ApplicationUser>();
        }
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Role Name is required")]
        public string RoleName { get; set; } = string.Empty;
        public string NormalizedName { get; set; } = string.Empty;

        public List<ApplicationUser> Users { get; set; }
    }
}

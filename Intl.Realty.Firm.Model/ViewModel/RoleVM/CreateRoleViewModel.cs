using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.RoleVM
{
    public class CreateRoleViewModel
    {
        [Required]
        public string RoleName { get; set; } = string.Empty;
    }
}

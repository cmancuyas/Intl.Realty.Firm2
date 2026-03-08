using Intl.Realty.Firm.Model.Model;
using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.InternalEmailVM
{
    public class InternalEmailViewModel : BaseModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The Role Name field is required.")]
        public Guid RoleId { get; set; }
        public string? RoleName { get; set; }
        [EmailAddress]
        [Required(ErrorMessage = "The Email field is required.")]
        public string Email { get; set; } = string.Empty;
    }
}

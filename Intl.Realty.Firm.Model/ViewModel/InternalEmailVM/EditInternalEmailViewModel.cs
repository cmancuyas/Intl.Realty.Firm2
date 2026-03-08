using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.ViewModel.RoleVM;
using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.InternalEmailVM
{
    public class EditInternalEmailViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The Role Name field is required.")]
        public Guid RoleId { get; set; }
        public string? RoleName { get; set; }
        [EmailAddress]
        [Required(ErrorMessage = "The Email field is required.")]
        public string Email { get; set; } = string.Empty;

        [Required]
        public bool IsActive { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<ApplicationRole>? RoleList { get; set; }
    }
}

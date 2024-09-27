using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.InternalEmailVM
{
    public class CreateInternalEmailViewModel
    {
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
        [Required]
        public Guid CreatedBy { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        public IEnumerable<SelectListItem>? RoleIEnum { get; set; }
    }
}

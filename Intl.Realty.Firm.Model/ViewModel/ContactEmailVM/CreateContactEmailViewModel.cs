using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.ContactEmailVM
{
    public class CreateContactEmailViewModel
    {
        [Required(ErrorMessage = "The Email Category field is required.")]
        public int? EmailCategoryId { get; set; }

        [Required(ErrorMessage = "The Email field is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; } = string.Empty;

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        [Required]
        public Guid CreatedBy { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public IEnumerable<SelectListItem>? EmailCategoryIEnum { get; set; }
    }

}

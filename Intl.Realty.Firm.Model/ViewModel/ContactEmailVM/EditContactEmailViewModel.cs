using Intl.Realty.Firm.Model.Model;
using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.ContactEmailVM
{
    public class EditContactEmailViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The Email Category field is required.")]
        [MaxLength(100)]
        public int EmailCategoryId { get; set; }
        [Required(ErrorMessage = "The Email field is required.")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public bool IsActive { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<EmailCategory>? EmailCategoryList { get; set; }
    }
}

using Intl.Realty.Firm.Model.Model;
using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.ContactEmailVM
{
    public class PreviewContactEmailViewModel
    {
        public int Id { get; set; }
        [MaxLength(100)]
        [Required(ErrorMessage = "The FirstName field is required.")]
        public string FirstName { get; set; } = string.Empty;
        [MaxLength(100)]
        [Required(ErrorMessage = "The LastName field is required.")]
        public string LastName { get; set; } = string.Empty;
        [Required(ErrorMessage = "The Email Category field is required.")]
        public int EmailCategoryId { get; set; }
        [Required(ErrorMessage = "The Email field is required.")]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public EmailCategory? EmailCategory { get; set; }
    }
}

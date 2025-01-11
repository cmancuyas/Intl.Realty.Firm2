using Intl.Realty.Firm.Model.Model;
using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.ContactEmailVM
{
    public class PreviewContactEmailViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The Email Category field is required.")]
        [MaxLength(100)]
        public int EmailCategoryId { get; set; }
        [Required(ErrorMessage = "The Email field is required.")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public EmailCategory? EmailCategory { get; set; }
    }
}

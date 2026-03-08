using Intl.Realty.Firm.Model.Model;
using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.ContactEmailVM
{
    public class ContactEmailViewModel : BaseModel
    {
        public int Id { get; set; }
        [Required]
        public int EmailCategoryId { get; set; }
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;
        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}

using Intl.Realty.Firm.Model.Model;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.ContactEmailVM
{
    public class ContactEmailViewModel : BaseModel
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public int EmailCategoryId { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}

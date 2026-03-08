using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.CustomerVM
{
    public class EditCustomerViewModel
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        [Phone]
        public string TelephoneNumber { get; set; } = string.Empty;
        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        public bool IsActive { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

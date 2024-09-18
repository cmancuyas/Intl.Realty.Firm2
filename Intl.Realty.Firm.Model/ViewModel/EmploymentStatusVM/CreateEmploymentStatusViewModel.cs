using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.EmploymentStatusVM
{
    public class CreateEmploymentStatusViewModel
    {
        [Required(ErrorMessage = "The Name field is required.")]
        public string Code { get; set; } = string.Empty;
        [Required(ErrorMessage = "The Description field is required.")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public bool IsActive { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
        [Required]
        public Guid CreatedBy { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
    }
}

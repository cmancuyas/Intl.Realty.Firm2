using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.EmailCategoryVM
{
    public class EditEmailCategoryViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The Name field is required.")]
        public string Code { get; set; } = string.Empty;
        [Required(ErrorMessage = "The Description field is required.")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public bool IsActive { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

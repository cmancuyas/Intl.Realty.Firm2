using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.DepartmentVM
{
    public class EditDepartmentViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The Name field is required.")]
        public string Name { get; set; } = string.Empty;
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

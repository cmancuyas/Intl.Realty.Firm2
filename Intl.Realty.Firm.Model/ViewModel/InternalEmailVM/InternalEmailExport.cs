using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.InternalEmailVM
{
    public class InternalEmailExport
    {
        public string? RoleName { get; set; }
        [Required(ErrorMessage = "The Email field is required.")]
        public string Email { get; set; } = string.Empty;
    }
}

using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.AccountVM
{
    public class SupportViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(15, ErrorMessage = "Your response must be at least 15 characters.")]
        public string Description { get; set; } = string.Empty;
    }

}

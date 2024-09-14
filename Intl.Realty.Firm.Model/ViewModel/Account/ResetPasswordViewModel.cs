using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.Account
{
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and Confirm Password must match")]
        public string ConfirmPassword { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}

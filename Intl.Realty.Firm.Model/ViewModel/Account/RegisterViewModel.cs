using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;


namespace Intl.Realty.Firm.Model.ViewModel.Account
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Remote(action:"IsEmailInUse",controller: "Account")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name="Confirm Password")]
        [Compare("Password",ErrorMessage ="Password and confirm password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;
    }
}

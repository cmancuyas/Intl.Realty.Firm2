using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.AccountVM
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

        [Required(ErrorMessage = "The FirstName field is required")]
        public string FirstName { get; set; } = string.Empty;
        [Required(ErrorMessage = "The LastName field is required")]
        public string LastName { get; set; } = string.Empty;
        [Required(ErrorMessage = "The Phone field is required")]
        public string Phone { get; set; } = string.Empty;
        public int? DepartmentId { get; set; }
        public int EmploymentStatusId { get; set; }
        public int? ProfilePictureId { get; set; }
        [Display(Name = "User Agreement")]
        //[Range(typeof(bool), "false", "true", ErrorMessage = "You must accept the terms and conditions.")]
        [Required(ErrorMessage = "You must accept the terms and conditions.")]
        public bool UserAgreement { get; set; } = false;
    }
}

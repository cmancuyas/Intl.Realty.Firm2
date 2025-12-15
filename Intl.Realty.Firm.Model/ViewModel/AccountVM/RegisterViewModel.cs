using Intl.Realty.Firm.Model.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.AccountVM
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "The Email field is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        [Remote(action: "IsEmailInUse", controller: "Account", ErrorMessage = "This email is already in use.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Password field is required.")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Confirm Password field is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and confirm password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "The First Name field is required.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Last Name field is required.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Phone field is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Department field is required.")]
        public int? DepartmentId { get; set; } = 1; // Default to 1 if not provided

        [Required(ErrorMessage = "The Employment Status field is required.")]
        public int EmploymentStatusId { get; set; } = 1; // Default employment status

        public int? ProfileImageId { get; set; }

        [Display(Name = "User Agreement")]
        [MustBeTrue(ErrorMessage = "You must accept the terms and conditions.")]
        public bool UserAgreement { get; set; } = false;

        //[Required(ErrorMessage = "Token is required for verification.")]
        public string? Token { get; set; }

        /// <summary>
        /// Determines if the user should be automatically signed in after registration.
        /// Default is false to require email verification before login.
        /// </summary>
        [Display(Name = "Sign in after registration")]
        public bool AutoSignInAfterRegister { get; set; } = false;
        // 🔹 New properties for TeamLeader selection
        [Display(Name = "Assign Team Leader")]
        public Guid? TeamLeaderId { get; set; } // Stores the selected TeamLeader ID

        public List<SelectListItem>? TeamLeaders { get; set; } // Stores the list of TeamLeaders
    }
}

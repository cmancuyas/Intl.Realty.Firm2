using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.AccountVM
{
    public class ResendConfirmationViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}

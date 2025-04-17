using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.Base.Concrete
{
    public class LandLordDetails
    {
        [Required(ErrorMessage = "Landlord's name is required.")]
        public string LandLordName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Landlord's phone number is required.")]
        public string LandLordPhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Landlord's email address is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address for the landlord.")]
        public string LandLordEmailAddress { get; set; } = string.Empty;
    }
}

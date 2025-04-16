using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.Base.Concrete
{
    public class LandLordDetails
    {
        // Landlord
        [Required(ErrorMessage = "The Landlord's Name field is required.")]
        public string LandLordName { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Landlord's Phone Number field is required.")]
        public string LandLordPhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Landlord's Email Address field is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address format.")]
        public string LandLordEmailAddress { get; set; } = string.Empty;
    }
}

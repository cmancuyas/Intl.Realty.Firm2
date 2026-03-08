using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.Base.Concrete
{
    public class LandLordDetails
    {
        [Required(ErrorMessage = "Landlord's name is required.")]
        public string LandLordName { get; set; } = string.Empty;

        public string? LandLordPhoneNumber { get; set; } = string.Empty;
        public string? LandLordEmailAddress { get; set; } = string.Empty;
    }
}

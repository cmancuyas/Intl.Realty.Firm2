using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.Base.Concrete
{
    public class ListingInfo
    {
        // Listing Info
        [Required(ErrorMessage = "The Listing Agent Name field is required.")]
        public string ListingAgentName { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Listing Brokerage field is required.")]
        public string ListingBrokerage { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Listing Brokerage Fax field is required.")]
        public string ListingBrokerageFax { get; set; } = string.Empty;
    }
}

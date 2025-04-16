using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.Base.Concrete
{
    public class BuyerDetails
    {
        // Buyer
        [Required(ErrorMessage = "The Buyer's Name field is required.")]
        public string BuyerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Buyer's Phone Number field is required.")]
        public string BuyersPhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Buyer's Lawyer Name field is required.")]
        public string BuyersLawyer { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Buyer's Lawyer Address field is required.")]
        public string BuyerLawyerAddress { get; set; } = string.Empty;
        [Required(ErrorMessage = "The Buyer's Email Address field is required.")]
        public string BuyerLawyerEmail { get; set; } = string.Empty;
        [Required(ErrorMessage = "The Buyer's Phone Address field is required.")]
        public string BuyerLawyerPhone { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Buyer's Agent Name field is required.")]
        public string BuyerAgentName { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Buyer's Brokerage field is required.")]
        public string BuyerBrokerage { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Buyer's Brokerage Fax field is required.")]
        public string BuyerBrokerageFax { get; set; } = string.Empty;
    }
}

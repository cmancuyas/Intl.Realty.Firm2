using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.Base.Concrete
{
    public class BuyerDetails
    {
        [Required(ErrorMessage = "Buyer's name is required.")]
        public string BuyerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Buyer's phone number is required.")]
        public string BuyersPhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Buyer's lawyer's name is required.")]
        public string BuyersLawyer { get; set; } = string.Empty;

        [Required(ErrorMessage = "Buyer's lawyer's address is required.")]
        public string BuyerLawyerAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "Buyer's lawyer's email address is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address for the buyer's lawyer.")]
        public string BuyerLawyerEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Buyer's lawyer's phone number is required.")]
        public string BuyerLawyerPhone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Buyer's agent name is required.")]
        public string BuyerAgentName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Buyer's brokerage is required.")]
        public string BuyerBrokerage { get; set; } = string.Empty;

        [Required(ErrorMessage = "Buyer's brokerage fax is required.")]
        public string BuyerBrokerageFax { get; set; } = string.Empty;
    }
}

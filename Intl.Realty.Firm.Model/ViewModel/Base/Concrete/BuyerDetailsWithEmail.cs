using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.Base.Concrete
{
    public class BuyerDetailsWithEmail : BuyerDetailsEmailOnly
    {
        [Required(ErrorMessage = "Buyer's name is required.")]
        public string BuyerName { get; set; } = string.Empty;
        public string? BuyerLawyer { get; set; }

        public string? BuyerLawyerAddress { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Enter a valid email address for the buyer's lawyer.")]
        public string? BuyerLawyerEmail { get; set; } 
        public string? BuyerLawyerPhone { get; set; }

        [Required(ErrorMessage = "Buyer's agent name is required.")]
        public string BuyerAgentName { get; set; } = string.Empty;

        public string? BuyerBrokerage { get; set; } 

        public string? BuyerBrokerageFax { get; set; } 
        public string? BuyerBrokeragePhoneNumber{ get; set; } 

        public string? BuyerBrokerageEmail { get; set; } 
    }
}

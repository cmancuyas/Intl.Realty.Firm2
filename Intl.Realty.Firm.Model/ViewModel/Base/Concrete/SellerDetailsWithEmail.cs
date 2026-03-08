using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.Base.Concrete
{
    public class SellerDetailsWithEmail : SellerDetailsEmailOnly
    {
        [Required(ErrorMessage = "Seller's Name Number is required.")]
        public string SellerName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Seller's Agent Name Number is required.")]
        public string SellerAgentName { get; set; } = string.Empty;

        public string? SellerLawyer { get; set; } 

        public string? SellerLawyerAddress { get; set; } 

        public string? SellerLawyerPhone { get; set; } 

        [EmailAddress(ErrorMessage = "Enter a valid email address for the seller's lawyer.")]
        public string? SellerLawyerEmail { get; set; }
        public string? SellerBrokerage { get; set; }
        public string? SellerBrokerageFax { get; set; }
        public string? SellerBrokeragePhoneNumber { get; set; }
        public string? SellerBrokerageEmail { get; set; }
    }
}

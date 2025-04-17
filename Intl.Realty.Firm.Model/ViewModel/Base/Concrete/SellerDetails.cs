using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.Base.Concrete
{
    public class SellerDetails
    {
        [Required(ErrorMessage = "Seller's phone number is required.")]
        public string SellersPhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Seller's lawyer name is required.")]
        public string SellersLawyer { get; set; } = string.Empty;

        [Required(ErrorMessage = "Seller's lawyer address is required.")]
        public string SellersLawyerAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "Seller's lawyer phone number is required.")]
        public string SellerLawyerPhone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Seller's lawyer email address is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address for the seller's lawyer.")]
        public string SellerLawyerEmail { get; set; } = string.Empty;
    }
}

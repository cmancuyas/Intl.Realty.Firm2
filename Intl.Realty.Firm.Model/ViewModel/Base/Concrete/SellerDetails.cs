using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.Base.Concrete
{
    public class SellerDetails
    {
        // Seller
        [Required(ErrorMessage = "The Seller's Lawyer Name field is required.")]
        public string SellersLawyer { get; set; } = string.Empty;
        [Required(ErrorMessage = "The Seller's Phone Number field is required.")]
        public string SellersPhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Seller's Lawyer Address field is required.")]
        public string SellersLawyerAddress { get; set; } = string.Empty;
        [Required(ErrorMessage = "The Seller's Lawyer Phone field is required.")]
        public string SellerLawyerPhone { get; set; } = string.Empty;
        [Required(ErrorMessage = "The Seller's Lawyer Email field is required.")]
        public string SellerLawyerEmail { get; set; } = string.Empty;
    }
}

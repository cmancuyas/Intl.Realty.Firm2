using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.Base.Concrete
{
    public class SellerDetailsEmailOnly
    {
        [Required(ErrorMessage = "Seller's Email is required.")]
        [EmailAddress]
        public string SellerEmail { get; set; } = string.Empty;

    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.Base.Concrete
{
    public class Commission
    {
        [Required(ErrorMessage = "The Listing Commission Percentage field is required.")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal ListingCommissionPercentage { get; set; }

        [Required(ErrorMessage = "The Buying Commission Percentage field is required.")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal BuyingCommissionPercentage { get; set; }
    }
}

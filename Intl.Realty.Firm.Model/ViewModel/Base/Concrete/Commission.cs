using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.Base.Concrete
{
    public class Commission
    {
        public int ListingCommissionSign { get; set; } 
        [Required(ErrorMessage = "The Total Commission field is required.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ListingCommission { get; set; }

        public int BuyingCommissionSign { get; set; } 
        [Required(ErrorMessage = "The Buying Commission field is required.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal BuyingCommission { get; set; }
    }
}

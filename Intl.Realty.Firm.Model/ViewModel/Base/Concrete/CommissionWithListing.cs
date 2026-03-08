using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.Base.Concrete
{
    public class CommissionWithListing : CommissionBase
    {
        public int ListingCommissionSign{ get; set; } 
        [Required(ErrorMessage = "The Listing Commission field is required.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ListingCommission { get; set; }
    }
}

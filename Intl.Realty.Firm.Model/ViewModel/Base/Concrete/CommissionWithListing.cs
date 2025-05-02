using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.Base.Concrete
{
    public class CommissionWithListing : CommissionBase
    {
        [Required(ErrorMessage = "The Listing Commission Percentage field is required.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ListingCommissionPercentage { get; set; }
    }
}

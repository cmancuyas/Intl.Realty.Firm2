using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.Base.Concrete
{
    public abstract class CommissionBase
    {
        [Required(ErrorMessage = "The Buying Commission Percentage field is required.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal BuyingCommissionPercentage { get; set; }
    }

}

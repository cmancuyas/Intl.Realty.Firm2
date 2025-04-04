using Intl.Realty.Firm.Model.ViewModel.TransactionTypeDetailsBaseVM;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.IRFDealVM
{
    public class CreateIRFDealSaleListingViewModel : CreateTransactionTypeDetailsBaseSaleViewModel
    {
        //public int SaleListingId { get; set; }
        [Required(ErrorMessage = "The Listing Commission Percentage field is required.")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal ListingCommissionPercentage { get; set; }
    }
}

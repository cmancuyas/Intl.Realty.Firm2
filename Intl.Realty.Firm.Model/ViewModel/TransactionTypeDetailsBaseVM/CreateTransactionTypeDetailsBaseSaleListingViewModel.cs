using Intl.Realty.Firm.Model.ViewModel.Base.Concrete;

namespace Intl.Realty.Firm.Model.ViewModel.TransactionTypeDetailsBaseVM
{
    public class CreateTransactionTypeDetailsBaseSaleListingViewModel : CreateTransactionTypeDetailsBaseSaleViewModel
    {
        public new CommissionWithListing Commission { get; set; } = new CommissionWithListing();
    }

}

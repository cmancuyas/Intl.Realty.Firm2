using Intl.Realty.Firm.Model.ViewModel.Base.Concrete;
using Intl.Realty.Firm.Model.ViewModel.TransactionTypeDetailsBaseVM.Base;

namespace Intl.Realty.Firm.Model.ViewModel.TransactionTypeDetailsBaseVM
{
    public class EditTransactionTypeDetailsBaseSaleListingViewModel : EditTransactionTypeDetailsBaseSaleViewModel
    {
        public new CommissionWithListing Commission { get; set; } = new CommissionWithListing();
    }
}

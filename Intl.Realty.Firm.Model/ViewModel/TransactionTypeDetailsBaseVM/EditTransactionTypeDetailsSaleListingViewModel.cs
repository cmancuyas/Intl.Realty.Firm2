using Intl.Realty.Firm.Model.ViewModel.Base.Concrete;
using Intl.Realty.Firm.Model.ViewModel.TransactionTypeDetailsBaseVM.Base;

namespace Intl.Realty.Firm.Model.ViewModel.TransactionTypeDetailsBaseVM
{
    public class EditTransactionTypeDetailsSaleListingViewModel : EditTransactionTypeDetailsBaseSaleListingViewModel
    {
        public new CommissionWithListing Commission { get; set; } = new CommissionWithListing();
    }
}

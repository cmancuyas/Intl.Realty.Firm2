using Intl.Realty.Firm.Model.ViewModel.TransactionTypeDetailsBaseVM.Base;
using Intl.Realty.Firm.Model.ViewModel.Base.Concrete;
namespace Intl.Realty.Firm.Model.ViewModel.TransactionTypeDetailsBaseVM
{
    public class EditTransactionTypeDetailsLeaseListingViewModel : EditTransactionTypeDetailsBaseLeaseViewModel
    {
        public new CommissionWithListing Commission { get; set; } = new CommissionWithListing();
    }
}


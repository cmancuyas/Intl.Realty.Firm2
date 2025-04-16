using Intl.Realty.Firm.Model.ViewModel.Base.Concrete;
using Intl.Realty.Firm.Model.ViewModel.TransactionTypeDetailsBaseVM.Base;

namespace Intl.Realty.Firm.Model.ViewModel.TransactionTypeDetailsBaseVM
{
    public class EditTransactionTypeDetailsBaseSaleCoopViewModel : EditTransactionTypeDetailsBaseSaleViewModel
    {
        public new CommissionWithoutListing Commission { get; set; } = new CommissionWithoutListing();
    }
}

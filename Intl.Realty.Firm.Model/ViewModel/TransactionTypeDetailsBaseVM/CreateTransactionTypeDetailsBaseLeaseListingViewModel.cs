using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Intl.Realty.Firm.Model.ViewModel.Base.Concrete;
using Intl.Realty.Firm.Model.ViewModel.TransactionTypeDetailsBaseVM.Base;

namespace Intl.Realty.Firm.Model.ViewModel.TransactionTypeDetailsBaseVM
{
    public class CreateTransactionTypeDetailsBaseLeaseListingViewModel : CreateTransactionTypeDetailsBaseLeaseViewModel
    {
        public new CommissionWithListing Commission { get; set; } = new CommissionWithListing();
    }
}

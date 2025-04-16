using Intl.Realty.Firm.Model.ViewModel.TransactionTypeDetailsBaseVM.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Intl.Realty.Firm.Model.ViewModel.Base.Concrete;
namespace Intl.Realty.Firm.Model.ViewModel.TransactionTypeDetailsBaseVM
{
    public class EditTransactionTypeDetailsBaseLeaseListingViewModel : EditTransactionTypeDetailsBaseLeaseViewModel
    {
        public new CommissionWithListing Commission { get; set; } = new CommissionWithListing();
    }
}


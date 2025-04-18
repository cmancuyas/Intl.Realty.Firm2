using Intl.Realty.Firm.Model.ViewModel.Base.Concrete;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intl.Realty.Firm.Model.ViewModel.TransactionTypeDetailsBaseVM
{
    public class CreateTransactionTypeDetailsBaseSaleCoopViewModel : CreateTransactionTypeDetailsBaseSaleViewModel
    {
        public new CommissionWithoutListing Commission { get; set; } = new CommissionWithoutListing();
    }
}

namespace Intl.Realty.Firm.Model.ViewModel.TransactionTypeVM
{
    public class DeleteTransactionTypeViewModel
    {
        public int Id { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}

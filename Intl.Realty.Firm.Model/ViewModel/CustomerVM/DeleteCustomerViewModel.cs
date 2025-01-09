namespace Intl.Realty.Firm.Model.ViewModel.CustomerVM
{
    public class DeleteCustomerViewModel
    {
        public int Id { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}

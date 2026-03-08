namespace Intl.Realty.Firm.Model.ViewModel.DealStatusVM
{
    public class DeleteDealStatusViewModel
    {
        public int Id { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}

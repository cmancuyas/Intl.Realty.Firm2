namespace Intl.Realty.Firm.Model.ViewModel.EmailStatusVM
{
    public class DeleteEmailStatusViewModel
    {
        public int Id { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}

namespace Intl.Realty.Firm.Model.ViewModel.ContactEmailVM
{
    public class DeleteContactEmailViewModel
    {
        public int Id { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}

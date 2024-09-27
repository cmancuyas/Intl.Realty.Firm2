namespace Intl.Realty.Firm.Model.ViewModel.InternalEmailVM
{
    public class DeleteInternalEmailViewModel
    {
        public int Id { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}

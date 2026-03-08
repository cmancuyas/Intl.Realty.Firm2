namespace Intl.Realty.Firm.Model.ViewModel.EmailCategoryVM
{
    public class DeleteEmailCategoryViewModel
    {
        public int Id { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}

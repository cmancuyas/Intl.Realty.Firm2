namespace Intl.Realty.Firm.Model.ViewModel.LeaseListingVM
{
    public class DeleteLeaseListingViewModel
    {
        public int Id { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedByFullName { get; set; }
    }
}

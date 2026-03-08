namespace Intl.Realty.Firm.Model.ViewModel.LeaseCoopVM
{
    public class DeleteLeaseCoopViewModel
    {
        public int Id { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedByFullName { get; set; }
    }
}

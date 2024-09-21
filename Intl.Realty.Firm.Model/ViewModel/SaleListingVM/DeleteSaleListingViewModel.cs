namespace Intl.Realty.Firm.Model.ViewModel.SaleListingVM
{
    public class DeleteSaleListingViewModel
    {
        public int Id { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}

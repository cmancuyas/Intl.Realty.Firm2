namespace Intl.Realty.Firm.Model.ViewModel.SaleCoopVM
{
    public class DeleteSaleCoopViewModel
    {
        public int Id { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedByFullName { get; set; }
    }
}

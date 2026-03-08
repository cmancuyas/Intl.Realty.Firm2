namespace Intl.Realty.Firm.Model.ViewModel.DocumentTypeVM
{
    public class DeleteDocumentTypeViewModel
    {
        public int Id { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}

namespace Intl.Realty.Firm.Model.ViewModel.DocumentTypeAssignmentVM
{
    public class DeleteDocumentTypeAssignmentViewModel
    {
        public int Id { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}

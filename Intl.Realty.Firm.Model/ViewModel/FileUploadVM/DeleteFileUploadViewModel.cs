namespace Intl.Realty.Firm.Model.ViewModel.FileUploadVM
{
    public class DeleteFileUploadViewModel
    {
        public int Id { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}

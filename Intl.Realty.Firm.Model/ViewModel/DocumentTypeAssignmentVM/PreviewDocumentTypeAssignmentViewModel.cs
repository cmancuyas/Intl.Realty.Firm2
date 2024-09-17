using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.DocumentTypeAssignmentVM
{
    public class PreviewDocumentTypeAssignmentViewModel
    {
        public int Id { get; set; }
        [Required]
        public int DocumentTypeId { get; set; }
        [Required]
        public int TransactionTypeId { get; set; }
    }
}

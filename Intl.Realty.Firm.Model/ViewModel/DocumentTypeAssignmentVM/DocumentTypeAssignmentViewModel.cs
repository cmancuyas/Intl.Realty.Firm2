using Intl.Realty.Firm.Model.Model;
using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.DocumentTypeAssignmentVM
{
    public class DocumentTypeAssignmentViewModel : BaseModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int DocumentTypeId { get; set; }
        [Required]
        public int TransactionTypeId { get; set; }
        public DocumentType? DocumentType { get; set; }
        public TransactionType? TransactionType { get; set; }
    }
}

using Intl.Realty.Firm.Model.Model;
using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.DocumentTypeAssignmentVM
{
    public class EditDocumentTypeAssignmentViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The DocumentType field is required.")]
        public int DocumentTypeId { get; set; }
        [Required(ErrorMessage = "The TransactionType field is required.")]
        public int TransactionTypeId { get; set; }
        [Required]
        public bool IsRequired { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<DocumentType>? DocumentTypeList { get; set; }
        public List<TransactionType>? TransactionTypeList { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.DocumentTypeAssignmentVM
{
    public class CreateDocumentTypeAssignmentViewModel
    {
        [Required(ErrorMessage = "The DocumentType field is required.")]
        public int DocumentTypeId { get; set; }
        
        [Required(ErrorMessage = "The TransactionType field is required.")]
        public int TransactionTypeId { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
        [Required]
        public Guid CreatedBy { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        public IEnumerable<SelectListItem>? DocumentTypeIEnum { get; set; }
        public IEnumerable<SelectListItem>? TransactionTypeIEnum { get; set; }
    }
}

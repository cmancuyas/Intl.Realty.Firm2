using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.Model
{
    public class DocumentTypeAssignment : BaseModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int DocumentTypeId { get; set; }
        [Required]
        public int TransactionTypeId { get; set; }
        public DocumentType? DocumentType { get; set; }
        public TransactionType? TransactionType { get; set; }
    }
}

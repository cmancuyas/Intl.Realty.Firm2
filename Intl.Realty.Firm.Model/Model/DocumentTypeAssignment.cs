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
        [ForeignKey("DocumentType")]
        public int DocumentTypeId { get; set; }
        [Required]
        [ForeignKey("TransactionType")]
        public int TransactionTypeId { get; set; }
        public bool IsRequired { get; set; } = false;
        public virtual DocumentType? DocumentType { get; set; }
        public virtual TransactionType? TransactionType { get; set; }
    }
}

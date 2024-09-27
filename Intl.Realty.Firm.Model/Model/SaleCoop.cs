using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.Model
{
    public class SaleCoop : BaseModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [ForeignKey("TransactionType")]
        public int TransactionTypeId { get; set; }
        [Required]
        [ForeignKey("IRFDeal")]
        public int IRFDealId { get; set; }
        [Required]
        [ForeignKey("DealStatus")]
        public int DealStatusId { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public List<FileUpload>? FileUploads { get; set; } = new List<FileUpload>();

        public virtual IRFDeal? IRFDeal { get; set; }
        public virtual TransactionType? TransactionType { get; set; }
        public virtual DealStatus? DealStatus { get; set; }
        public string? Notes { get; set; }
        public string CreatedByFullName { get; set; } = string.Empty;
        public string? UpdatedByFullName { get; set; } = string.Empty;
        public string? DeletedByFullName { get; set; } = string.Empty;
    }
}

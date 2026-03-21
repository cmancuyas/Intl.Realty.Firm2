using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.Model
{
    public class LeaseCoop : BaseModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string DealTransactionNumber { get; set; } = string.Empty;
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
        public List<AdditionalDocument>? AdditionalDocuments { get; set; } = new List<AdditionalDocument>();
        public virtual IRFDeal? IRFDeal { get; set; }
        public virtual TransactionType? TransactionType { get; set; }
        public virtual DealStatus? DealStatus { get; set; }
        public string? Notes { get; set; }
        public string CreatedByFullName { get; set; } = string.Empty;
        public string? UpdatedByFullName { get; set; } = string.Empty;
        public string? DeletedByFullName { get; set; } = string.Empty;
        // 🔒 Add this for concurrency control
        [Timestamp]
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    }
}

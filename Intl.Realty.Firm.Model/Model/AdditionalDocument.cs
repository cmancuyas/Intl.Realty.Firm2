using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intl.Realty.Firm.Model.Model
{
    public class AdditionalDocument : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string DocumentName { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;

        public string FullPath { get; set; } = string.Empty;

        public string OriginalFileName { get; set; } = string.Empty;

        public string Directory { get; set; } = string.Empty;

        public string FileExtension { get; set; } = string.Empty;

        public string FileSize { get; set; } = string.Empty;


        // ===== Foreign Keys =====

        [ForeignKey("SaleListing")]
        public int? SaleListingId { get; set; }

        [ForeignKey("SaleCoop")]
        public int? SaleCoopId { get; set; }

        [ForeignKey("LeaseListing")]
        public int? LeaseListingId { get; set; }

        [ForeignKey("LeaseCoop")]
        public int? LeaseCoopId { get; set; }

        [ForeignKey("TransactionType")]
        public int TransactionTypeId { get; set; }


        // ===== Navigation =====

        public virtual SaleListing? SaleListing { get; set; }

        public virtual SaleCoop? SaleCoop { get; set; }

        public virtual LeaseListing? LeaseListing { get; set; }

        public virtual LeaseCoop? LeaseCoop { get; set; }

        public virtual TransactionType? TransactionType { get; set; }


        // ===== Concurrency =====

        [Timestamp]
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    }
}
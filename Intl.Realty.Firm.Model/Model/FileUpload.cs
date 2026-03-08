using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.Model
{
    public class FileUpload : BaseModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FullPath { get; set; } = string.Empty;
        public string OriginalFileName { get; set; } = string.Empty;
        public string Directory { get; set; } = string.Empty;
        public string FileExtension { get; set; } = string.Empty;
        public string FileSize { get; set; } = string.Empty;
        [ForeignKey("SaleListing")]
        public int? SaleListingId { get; set; }
        [ForeignKey("SaleCoop")]
        public int? SaleCoopId { get; set; }
        [ForeignKey("LeaseListing")]
        public int? LeaseListingId { get; set; }
        [ForeignKey("LeaseCoop")]
        public int? LeaseCoopId { get; set; }
        [ForeignKey("DocumentType")]
        public int DocumentTypeId { get; set; }
        [ForeignKey("TransactionType")]
        public int TransactionTypeId { get; set; }
        public virtual SaleListing? SaleListing { get; set; }
        public virtual SaleCoop? SaleCoop { get; set; }
        public virtual LeaseCoop? LeaseCoop { get; set; }
        public virtual LeaseListing? LeaseListing { get; set; }
        public virtual DocumentType? DocumentType { get; set; }
        public virtual TransactionType? TransactionType { get; set; }
    }
}

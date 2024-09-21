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
        public int? SaleListingId { get; set; }
        public int? SaleCoopId { get; set; }
        public int? LeaseListingId { get; set; }
        public int? LeaseCoopId { get; set; }
        public int DocumentTypeId { get; set; }
        public int TransactionTypeId { get; set; }
        public SaleListing? SaleListing { get; set; }
        public SaleCoop? SaleCoop { get; set; }
        public LeaseCoop? LeaseCoop { get; set; }
        public LeaseListing? LeaseListing { get; set; }
        public DocumentType? DocumentType { get; set; }
        public TransactionType? TransactionType { get; set; }
    }
}

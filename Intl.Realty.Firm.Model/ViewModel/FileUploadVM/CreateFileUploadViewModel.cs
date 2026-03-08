using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.FileUploadVM
{
    public class CreateFileUploadViewModel
    {
        public string FileName { get; set; } = string.Empty;
        public string FullPath { get; set; } = string.Empty;
        public string Directory { get; set; } = string.Empty;
        public string FileExtension { get; set; } = string.Empty;
        public string FileSize { get; set; } = string.Empty;
        public int? DocumentTypeId { get; set; }
        public int? SaleListingId { get; set; }
        public int? TransactionTypeId { get; set; }
        public bool IsActive { get; set; }
        [Required]
        public int CreatedBy { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }

        public List<IFormFile>? Files { get; set; }
    }
}

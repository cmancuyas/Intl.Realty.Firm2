using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intl.Realty.Firm.Model.Model
{
    public class ProfileImage : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string FileName { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string FullPath { get; set; } = string.Empty;

        [MaxLength(255)]
        public string OriginalFileName { get; set; } = string.Empty;

        [MaxLength(20)]
        public string FileExtension { get; set; } = string.Empty;

        [MaxLength(50)]
        public string FileSize { get; set; } = string.Empty;

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        public virtual ApplicationUser? User { get; set; }

    }
}

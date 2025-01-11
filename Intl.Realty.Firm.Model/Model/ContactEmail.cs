using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.Model
{
    public class ContactEmail : BaseModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public int EmailCategoryId { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public EmailCategory? EmailCategory { get; set; }
    }
}

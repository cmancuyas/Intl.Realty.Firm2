using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.Model
{
    public class InternalEmail : BaseModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]  
        public Guid RoleId { get; set; }
        [Required]
        public string RoleName { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
    }
}

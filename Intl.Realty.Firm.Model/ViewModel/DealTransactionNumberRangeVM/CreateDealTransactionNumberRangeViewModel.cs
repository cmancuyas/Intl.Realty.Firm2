using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.DealTransactionNumberRangeVM
{
    public class CreateDealTransactionNumberRangeViewModel
    {
        [Required(ErrorMessage = "The Year field is required.")]
        public int Year { get; set; } = DateTime.Now.Year;
        [Required(ErrorMessage = "The Current Number field is required.")]
        public int CurrentNumber { get; set; }
        [MaxLength(15)]
        [Required(ErrorMessage = "The Number Range field is required.")]
        public string NumberRange { get; set; } = string.Empty;
        [Timestamp]
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
        [Required]
        public Guid CreatedBy { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
    }
}

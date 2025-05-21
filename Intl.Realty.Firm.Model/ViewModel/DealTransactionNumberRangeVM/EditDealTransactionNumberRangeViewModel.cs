using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.DealTransactionNumberRangeVM
{
    public class EditDealTransactionNumberRangeViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The Year field is required.")]
        public int Year { get; set; }
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
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

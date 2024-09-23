using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.Model
{
    public class LeaseCoop : BaseModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int TransactionTypeId { get; set; }
        public int IRFDealId { get; set; }
        public int DealStatusId { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public List<FileUpload>? FileUploads { get; set; }
        
        public IRFDeal? IRFDeal { get; set; }
        public TransactionType? TransactionType { get; set; }
        public DealStatus? DealStatus { get; set; }
    }
}

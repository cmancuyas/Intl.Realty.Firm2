using Intl.Realty.Firm.Model.Model;
using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.DealTransactionNumberRangeVM
{
    public class DealTransactionNumberRangeViewModel : BaseModel
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public int CurrentNumber { get; set; }
        public string NumberRange { get; set; } = string.Empty;
        [Timestamp]
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    }
}

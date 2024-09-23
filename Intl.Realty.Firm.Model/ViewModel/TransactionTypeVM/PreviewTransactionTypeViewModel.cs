using Intl.Realty.Firm.Model.Model;

namespace Intl.Realty.Firm.Model.ViewModel.TransactionTypeVM
{
    public class PreviewTransactionTypeViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<DocumentType> DocumentTypes { get; set; } = new List<DocumentType>();
    }
}

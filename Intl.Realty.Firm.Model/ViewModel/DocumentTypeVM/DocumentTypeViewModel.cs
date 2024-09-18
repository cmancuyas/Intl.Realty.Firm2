using Intl.Realty.Firm.Model.Model;

namespace Intl.Realty.Firm.Model.ViewModel.DocumentTypeVM
{
    public class DocumentTypeViewModel : BaseModel
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}

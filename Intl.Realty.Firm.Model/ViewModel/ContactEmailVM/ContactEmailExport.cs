using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.ContactEmailVM
{
    public class ContactEmailExport
    {
        public string EmailCategory { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}

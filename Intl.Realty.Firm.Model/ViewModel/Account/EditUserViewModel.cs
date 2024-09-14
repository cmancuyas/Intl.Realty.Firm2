using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Model.ViewModel.Account
{
    public class EditUserViewModel
    {
        [Required]
        public string Id { get; set; } = string.Empty;
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;
        public List<string> Claims { get; set; } = new List<string>();
        public List<string> Roles { get; set; } = new List<string>();
    }
}

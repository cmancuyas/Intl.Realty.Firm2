using System.ComponentModel.DataAnnotations;

namespace Intl.Realty.Firm.Models.Enum
{
    public enum Signage
    {
        [Display(Name = "%")]
        Percentage = 1,
        [Display(Name = "$")]
        USD = 2,
    }
}

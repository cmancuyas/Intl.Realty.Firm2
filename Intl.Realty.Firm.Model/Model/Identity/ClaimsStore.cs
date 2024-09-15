using System.Security.Claims;

namespace Intl.Realty.Firm.Model.Model.Identity
{
    public class ClaimsStore
    {
        public static List<Claim> AllClaims { get; set; } = new List<Claim>()
        {
            new Claim("Create File","Create File"),
            new Claim("Edit File","Edit File"),
            new Claim("Delete File","Delete File"),

            new Claim("Submit File","Submit File"),
        };
    }
}

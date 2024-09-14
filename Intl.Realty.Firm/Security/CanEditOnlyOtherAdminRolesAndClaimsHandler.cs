using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Intl.Realty.Firm.Security
{
    public class CanEditOnlyOtherAdminRolesAndClaimsHandler :
                AuthorizationHandler<ManageAdminRolesAndClaimsRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CanEditOnlyOtherAdminRolesAndClaimsHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                ManageAdminRolesAndClaimsRequirement requirement)
        {

            var loggedInAdminId = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value.ToString();

            var adminIdBeingEdited = _httpContextAccessor.HttpContext!.Request.Query["userId"].ToString();

            if (context.User.IsInRole("Admin")
                 && context.User.HasClaim(c => c.Type == "Edit Role" && c.Value == "true")
                 && adminIdBeingEdited.ToLower() == loggedInAdminId.ToLower())
            {
                context.Succeed(requirement);
            }
            if (context.User.IsInRole("Admin")
                 && context.User.HasClaim(c => c.Type == "Delete Role" && c.Value == "true")
                 && adminIdBeingEdited.ToLower() == loggedInAdminId.ToLower())
            {
                context.Succeed(requirement);
            }


            if (context.User.IsInRole("Admin")
                 && context.User.HasClaim(c => c.Type == "Create Role" && c.Value == "true")
                 && adminIdBeingEdited.ToLower() == loggedInAdminId.ToLower())
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }

    }
}

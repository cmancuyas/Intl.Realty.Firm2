using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Intl.Realty.Firm.Security
{
    public class SuperAdminHandler : AuthorizationHandler<ManageAdminRolesAndClaimsRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SuperAdminHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ManageAdminRolesAndClaimsRequirement requirement)
        {
            var loggedInAdminId = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value.ToString();

            var adminIdBeingEdited = _httpContextAccessor.HttpContext!.Request.Query["userId"].ToString();

            if (context.User.IsInRole("SuperAdmin"))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}

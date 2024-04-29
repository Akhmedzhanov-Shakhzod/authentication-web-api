using AuthenticationWebApi.Helpers.ApplicationException;
using AuthenticationWebApi.Helpers.Constant;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationWebApi.Helpers.Attribute
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : System.Attribute, IAuthorizationFilter
    {
        private readonly List<string> _roles = new List<string>();

        public AuthorizeAttribute()
        {
        }
        public AuthorizeAttribute(params string[] roles)
        {
            _roles = roles.ToList();
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // skip authorization if action is decorated with [AllowAnonymous] attribute
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
                return;

            // authorization
            var accountId = (string)context.HttpContext.Items["AccountId"];
            if (accountId == null)
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
            else if (_roles.Count > 0)
            {
                var serviceProvider = context.HttpContext.RequestServices;
                var userRoles = (List<string>)context.HttpContext.Items["AccountRoles"];

                if (userRoles == null || !userRoles.Any(ur => ur == AppConstants.Role_AdminRole || _roles.Contains(ur)))
                {
                    throw new ForbiddenException("Доступ запрещен :(");
                }
            }
        }
    }
}

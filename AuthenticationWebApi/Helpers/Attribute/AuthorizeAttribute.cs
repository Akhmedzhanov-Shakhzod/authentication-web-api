using AuthenticationWebApi.Helpers.ApplicationException;
using AuthenticationWebApi.Helpers.Constant;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using AuthenticationWebApi.Models.Account;
using AuthenticationWebApi.Mapper.Account;

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
            var account = (Account)context.HttpContext.Items["Account"];
            if (account == null)
            {
                // not logged in or role not authorized
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
            else if (_roles.Count > 0)
            {
                var serviceProvider = context.HttpContext.RequestServices;
                var _mapper = serviceProvider.GetRequiredService(typeof(IRoleMapper)) as IRoleMapper;
                var userRoles = _mapper.ToRoles(account);

                if (!userRoles.Any(ur => ur == AppConstants.Role_AdminRole || _roles.Contains(ur)))
                {
                    throw new ForbiddenException("Доступ запрещен :(");
                }
            }
        }
    }
}

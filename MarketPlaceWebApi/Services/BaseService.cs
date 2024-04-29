using User = AuthenticationWebApi.Models.Account.Account;

namespace AuthenticationWebApi.Services
{
    public class BaseService(IHttpContextAccessor httpContextAccessor)
    {
        protected User getCurrentAccount() => (User)httpContextAccessor.HttpContext.Items["Account"];
    }
}

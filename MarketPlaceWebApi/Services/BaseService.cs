using User = MarketPlaceWebApi.Models.Account.Account;

namespace MarketPlaceWebApi.Services
{
    public class BaseService(IHttpContextAccessor httpContextAccessor)
    {
        protected User getCurrentAccount() => (User)httpContextAccessor.HttpContext.Items["Account"];
    }
}

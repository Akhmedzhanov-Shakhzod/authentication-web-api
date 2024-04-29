namespace AuthenticationWebApi.Services
{
    public class BaseService(IHttpContextAccessor httpContextAccessor)
    {
        protected string getCurrentAccountId() => (string)httpContextAccessor.HttpContext.Items["AccountId"];
    }
}

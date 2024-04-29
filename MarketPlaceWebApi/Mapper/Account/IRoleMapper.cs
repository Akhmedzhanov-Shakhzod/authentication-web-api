using User = MarketPlaceWebApi.Models.Account.Account;

namespace MarketPlaceWebApi.Mapper.Account
{
    public interface IRoleMapper
    {
        string ToRole(User account);

        List<string> ToRoles(User account);
    }
}

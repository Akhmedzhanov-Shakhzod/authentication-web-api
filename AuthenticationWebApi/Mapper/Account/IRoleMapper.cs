using User = AuthenticationWebApi.Models.Account.Account;

namespace AuthenticationWebApi.Mapper.Account
{
    public interface IRoleMapper
    {
        string ToRole(User account);

        List<string> ToRoles(User account);
    }
}

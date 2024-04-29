using User = AuthenticationWebApi.Models.Account.Account;

namespace AuthenticationWebApi.Mappers.Account
{
    public interface IRoleMapper
    {
        string ToRole(User account);

        List<string> ToRoles(User account);
    }
}

using User = MarketPlaceWebApi.Models.Account.Account;
using Microsoft.AspNetCore.Identity;

namespace MarketPlaceWebApi.Mapper.Account.Impl
{
    public class DefaultRoleMapper(UserManager<User> userManager) : IRoleMapper
    {
        public string ToRole(User account)
        {
            var accountRoles = userManager.GetRolesAsync(account).Result;

            return accountRoles.Last();
        }

        public List<string> ToRoles(User account)
        {
            var accountRoles = userManager.GetRolesAsync(account).Result;

            return accountRoles.ToList();
        }
    }
}

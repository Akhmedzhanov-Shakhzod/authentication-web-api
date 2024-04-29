using AuthenticationWebApi.Helpers.Constant;
using AuthenticationWebApi.Models.Account;
using Microsoft.AspNetCore.Identity;

namespace AuthenticationWebApi.Helpers.DbContext
{
    public class DefaultInitializer
    {
        public static async Task InitializeAsync(UserManager<Account> userManager, RoleManager<Role> roleManager, AppPostgreSQLDbContext context)
        {
            if (await roleManager.FindByNameAsync(AppConstants.Role_AdminRole) == null)
            {
                await roleManager.CreateAsync(new Role { Name = AppConstants.Role_AdminRole });
            }
            if (await roleManager.FindByNameAsync(AppConstants.Role_ModeratorRole) == null)
            {
                await roleManager.CreateAsync(new Role { Name = AppConstants.Role_ModeratorRole });
            }
            if (await userManager.FindByNameAsync(AppConstants.Account_AdminEmail) == null)
            {
                Account admin = new Account
                {
                    Email = AppConstants.Account_AdminEmail,
                    UserName = AppConstants.Account_AdminEmail,
                    Surname = "Default",
                    Name = "Administrator",
                    CreatedAt = DateTime.Now
                };
                IdentityResult result = await userManager.CreateAsync(admin, AppConstants.Account_AdminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, AppConstants.Role_ModeratorRole);
                    await userManager.AddToRoleAsync(admin, AppConstants.Role_AdminRole);
                }
            }
        }

    }
}

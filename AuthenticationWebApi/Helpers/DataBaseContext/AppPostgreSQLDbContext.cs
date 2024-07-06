using AuthenticationWebApi.Models.Account;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationWebApi.Helpers.DataBaseContext
{
    public class AppPostgreSQLDbContext : IdentityDbContext<Account, Role, string>
    {
        public AppPostgreSQLDbContext(DbContextOptions options)
            : base(options) 
        {
        }
    }
}

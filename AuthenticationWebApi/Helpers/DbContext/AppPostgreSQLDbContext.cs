using AuthenticationWebApi.Models.Account;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationWebApi.Helpers.DbContext
{
    public class AppPostgreSQLDbContext : IdentityDbContext<Account, Role, string>
    {
        public AppPostgreSQLDbContext(DbContextOptions options)
            : base(options) 
        {
        }
    }
}

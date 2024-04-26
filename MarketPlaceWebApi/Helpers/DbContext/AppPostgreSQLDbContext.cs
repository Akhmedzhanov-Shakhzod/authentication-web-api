using MarketPlaceWebApi.Models.Account;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MarketPlaceWebApi.Helpers.DbContext
{
    public class AppPostgreSQLDbContext : IdentityDbContext<Account, Role, string>
    {
        public AppPostgreSQLDbContext(DbContextOptions options)
            : base(options) 
        {
        }
    }
}

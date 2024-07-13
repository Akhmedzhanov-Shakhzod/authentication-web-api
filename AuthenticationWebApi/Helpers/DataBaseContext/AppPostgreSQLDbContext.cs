using AuthenticationWebApi.Helpers.Enums;
using AuthenticationWebApi.Models.Account;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationWebApi.Helpers.DataBaseContext
{
    public class AppPostgreSQLDbContext : IdentityDbContext<Account, Role, string>
    {
        public DbSet<AccountSettings> AccountSettings { get; set; }


        public AppPostgreSQLDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AccountSettings>()
            .Property(e => e.Language)
            .HasConversion(
                v => v.ToString(),
                v => (Language)Enum.Parse(typeof(Language), v));

            modelBuilder.Entity<AccountSettings>()
                .HasIndex(a => a.Link)
                .IsUnique();

            modelBuilder.Entity<Account>()
                .Navigation(a => a.Settings)
                .AutoInclude();
        }
    }
}

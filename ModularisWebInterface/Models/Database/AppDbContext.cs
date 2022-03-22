using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ModularisWebInterface.Models.Database
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }
        public DbSet<UserConfirmation> UserConfirmation { get; set; }
        public DbSet<ModularisUserClaims> ModularisUserClaims { get; set; }
    }
}

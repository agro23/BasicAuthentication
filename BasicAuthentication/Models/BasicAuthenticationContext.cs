using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BasicAuthentication.Models
{
    public class BasicAuthenticationContext : IdentityDbContext<ApplicationUser>
    {
        public BasicAuthenticationContext(DbContextOptions options) : base(options)
        {

        }

       public DbSet<Item> Items { get; set; }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

                protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
                    => optionsBuilder
                        .UseMySql(@"Server=localhost;Port=8889;database=basicauthentication;uid=root;pwd=root;");

        public BasicAuthenticationContext(DbContextOptions<BasicAuthenticationContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>(entity => {
                entity.Property(m => m.Email).HasMaxLength(127);
                entity.Property(m => m.NormalizedEmail).HasMaxLength(127);
                entity.Property(m => m.NormalizedUserName).HasMaxLength(127);
                entity.Property(m => m.UserName).HasMaxLength(127);
            });
            builder.Entity<IdentityRole>(entity => {
                entity.Property(m => m.Name).HasMaxLength(127); entity.Property(m => m.NormalizedName).HasMaxLength(127);
            });
        }
    }
}
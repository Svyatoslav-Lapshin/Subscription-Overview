using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SubscriptionOverview.Api.Models;
using SubscriptionOverview.Api.Models.Identity;

namespace SubscriptionOverview.Api.Data
{
    public class SubscriptionOverviewDbContext : IdentityDbContext<ApplicationUser>
    {

        public SubscriptionOverviewDbContext(DbContextOptions<SubscriptionOverviewDbContext> options) : base(options)
        {



        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Subscription>()
                        .HasOne(c => c.Category)
                        .WithMany(s=>s.Subscriptions)
                        .HasForeignKey(c => c.CategoryId)
                        .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Subscription>()
                        .HasOne(c => c.Provider)
                        .WithMany(s => s.Subscriptions)
                        .HasForeignKey(c => c.ProviderId)
                        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Subscription>()
                        .Property(p => p.Price)
                        .HasPrecision(18, 2);


            modelBuilder.Entity<ApplicationUser>()
                        .HasMany(c => c.Categories)
                        .WithOne(u => u.User)
                        .HasForeignKey(u => u.UserId)
                         .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ApplicationUser>()
                       .HasMany(s => s.Subscriptions)
                       .WithOne(u => u.User)
                       .HasForeignKey(u => u.UserId)
                       .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<ApplicationUser>()
                     .HasMany(u => u.CustomProviders)
                     .WithOne(u => u.User)
                     .HasForeignKey(u => u.UserId )
                     .IsRequired(false)
                     .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ApplicationUser>()
                        .HasMany(u => u.RefreshTokens)
                        .WithOne(u => u.User)
                        .HasForeignKey(u => u.UserId)
                        .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<RefreshToken>()
                        .HasIndex(r => r.TokenHash)
                        .IsUnique();


        }
    }
}

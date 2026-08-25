using Microsoft.EntityFrameworkCore;
using SubscriptionOverview.Api.Models;

namespace SubscriptionOverview.Api.Data
{
    public class SubscriptionOverviewDbContext : DbContext
    {

        public SubscriptionOverviewDbContext(DbContextOptions<SubscriptionOverviewDbContext> options) : base(options)
        {



        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }

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
                        

        }
    }
}

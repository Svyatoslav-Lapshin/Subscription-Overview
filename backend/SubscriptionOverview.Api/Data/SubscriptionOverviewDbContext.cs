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
                        .WithMany(s => s.Subscriptions)
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
                        .IsRequired(false)
                        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ApplicationUser>()
                       .HasMany(s => s.Subscriptions)
                       .WithOne(u => u.User)
                       .HasForeignKey(u => u.UserId)
                       .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<ApplicationUser>()
                        .HasMany(u => u.CustomProviders)
                        .WithOne(u => u.User)
                        .HasForeignKey(u => u.UserId)
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

            //Prevent duplicate category names for the same user
            modelBuilder.Entity<Category>()
                        .HasIndex(c => new { c.CategoryName, c.UserId })
                        .IsUnique()
                        .HasFilter("[UserId] IS NOT NULL");

            //Prevent duplicate global category names
            modelBuilder.Entity<Category>()
                        .HasIndex(c => c.CategoryName)
                        .IsUnique()
                        .HasFilter("[UserId] IS NULL");

            //Prevent duplicate provider names for the same user
            modelBuilder.Entity<Provider>()
                        .HasIndex(p => new { p.ServiceName, p.UserId })
                        .IsUnique()
                        .HasFilter("[UserId] IS NOT NULL");

            //Prevent duplicate global provider names
            modelBuilder.Entity<Provider>()
                        .HasIndex(c => c.ServiceName)
                        .IsUnique()
                        .HasFilter("[UserId] IS NULL");


            //Seed global categories 
            modelBuilder.Entity<Category>()
                        .HasData(
                new Category
                {
                    Id = 1,
                    CategoryName = "Streaming",
                    UserId = null
                },
                new Category
                {
                    Id = 2,
                    CategoryName = "Music",
                    UserId = null
                },
                new Category
                {
                    Id = 3,
                    CategoryName = "Software",
                    UserId = null
                },
                new Category
                {
                    Id = 4,
                    CategoryName = "Cloud",
                    UserId = null
                },
                new Category
                {
                    Id = 5,
                    CategoryName = "Productivity",
                    UserId = null
                },
                new Category
                {
                    Id = 6,
                    CategoryName = "Gaming",
                    UserId = null
                },
                new Category
                {
                    Id = 7,
                    CategoryName = "Other",
                    UserId = null
                });


            modelBuilder.Entity<Provider>()
                        .HasData(
                        //Streaming
                        new Provider
                        {
                            Id = 1,
                            ServiceName = "Netflix",
                            UserId = null,
                            IsCustom = false
                        },
                        new Provider
                        {
                            Id = 2,
                            ServiceName = "Disney+",
                            UserId = null,
                            IsCustom = false
                        },
                        new Provider
                        {
                            Id = 3,
                            ServiceName = "HBO Max",
                            UserId = null,
                            IsCustom = false
                        },
                        new Provider
                        {
                            Id = 4,
                            ServiceName = "YouTube Premium",
                            UserId = null,
                            IsCustom = false
                        },
                        //Music
                        new Provider
                        {
                            Id = 5,
                            ServiceName = "Spotify",
                            UserId = null,
                            IsCustom = false
                        },
                        new Provider
                        {
                            Id = 6,
                            ServiceName = "Apple Music",
                            UserId = null,
                            IsCustom = false
                        },
                        new Provider
                        {
                            Id = 7,
                            ServiceName = "YouTube Music",
                            UserId = null,
                            IsCustom = false
                        },
                        //Software
                        new Provider
                        {
                            Id = 8,
                            ServiceName = "Microsoft 365",
                            UserId = null,
                            IsCustom = false
                        },
                        new Provider
                        {
                            Id = 9,
                            ServiceName = "Adobe Creative Cloud",
                            UserId = null,
                            IsCustom = false
                        },
                        new Provider
                        {
                            Id = 10,
                            ServiceName = "JetBrains",
                            UserId = null,
                            IsCustom = false
                        },
                        //Cloud
                        new Provider
                        {
                            Id = 11,
                            ServiceName = "Google One",
                            UserId = null,
                            IsCustom = false
                        },
                        new Provider
                        {
                            Id = 12,
                            ServiceName = "Dropbox",
                            UserId = null,
                            IsCustom = false
                        },
                        new Provider
                        {
                            Id = 13,
                            ServiceName = "iCloud+",
                            UserId = null,
                            IsCustom = false
                        },
                        //Productivity
                        new Provider
                        {
                            Id = 14,
                            ServiceName = "Notion",
                            UserId = null,
                            IsCustom = false
                        },
                        new Provider
                        {
                            Id = 15,
                            ServiceName = "Todoist",
                            UserId = null,
                            IsCustom = false
                        },
                        //Gaming
                        new Provider
                        {
                            Id = 16,
                            ServiceName = "Xbox Game Pass",
                            UserId = null,
                            IsCustom = false
                        },
                        new Provider
                        {
                            Id = 17,
                            ServiceName = "PlayStation Plus",
                            UserId = null,
                            IsCustom = false
                        },
                        new Provider
                        {
                            Id = 18,
                            ServiceName = "GeForce NOW",
                            UserId = null,
                            IsCustom = false
                        }

                );

        }
    }
}

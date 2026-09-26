using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SubscriptionOverview.Api.Models;
using SubscriptionOverview.Api.Models.Enums;
using SubscriptionOverview.Api.Models.Identity;

namespace SubscriptionOverview.Api.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(SubscriptionOverviewDbContext context, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            //Get demo credentials from configuration
            var demoEmail = configuration["DemoUser:Email"];
            var demoPassword = configuration["DemoUser:Password"];

            //Check configuration values
            if (string.IsNullOrWhiteSpace(demoEmail) || string.IsNullOrWhiteSpace(demoPassword))
            {
                throw new InvalidOperationException("Demo user configuration is missing");
            }

            //Check if demo user already exists
            var demoUser = await userManager.FindByEmailAsync(demoEmail);

            //Create demo user if missing
            if (demoUser == null)
            {
                demoUser = new ApplicationUser
                {
                    UserName = demoEmail,
                    Email = demoEmail,
                    FirstName = "Demo",
                    LastName = "User"
                };

                var result = await userManager.CreateAsync(demoUser, demoPassword);

                if (!result.Succeeded)
                {

                    throw new InvalidOperationException("Failed to create demo user.");
                }
            }

            var streamingCategory = await context.Categories.FirstOrDefaultAsync(c => c.CategoryName == "Streaming" && c.UserId == null);

            var netflixProvider = await context.Providers.FirstOrDefaultAsync(p => p.ServiceName == "Netflix" && p.UserId == null);

            if (streamingCategory == null || netflixProvider == null)
            {
                throw new InvalidOperationException("Required seed category or provider was not found");
            }

            var netflixSubscriptionExists = await context.Subscriptions.AnyAsync(s => s.UserId == demoUser.Id && s.ProviderId == netflixProvider.Id);

            if (!netflixSubscriptionExists)
            {

                var newNetflixSubscription = new Subscription
                {
                    UserId = demoUser.Id,
                    CategoryId = streamingCategory.Id,
                    ProviderId = netflixProvider.Id,
                    Price = 149m,
                    BillingInterval = BillingInterval.Monthly,
                    StartDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(-3)),
                    EndDate = null

                };

                await context.Subscriptions.AddAsync(newNetflixSubscription);
            }

            var musicCategory = await context.Categories.FirstOrDefaultAsync(c => c.CategoryName == "Music" && c.UserId == null);

            var spotifyProvider = await context.Providers.FirstOrDefaultAsync(p => p.ServiceName == "Spotify" && p.UserId == null);

            if (musicCategory == null || spotifyProvider == null)
            {
                throw new InvalidOperationException("Required seed category or provider was not found");
            }

            var spotifySubscriptionExists = await context.Subscriptions.AnyAsync(s => s.UserId == demoUser.Id && s.ProviderId == spotifyProvider.Id);

            if (!spotifySubscriptionExists)
            {

                var newSpotifySubscription = new Subscription
                {
                    UserId = demoUser.Id,
                    CategoryId = musicCategory.Id,
                    ProviderId = spotifyProvider.Id,
                    Price = 119m,
                    BillingInterval = BillingInterval.Monthly,
                    StartDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(-6)),
                    EndDate = null

                };

                await context.Subscriptions.AddAsync(newSpotifySubscription);
            }

            var softwareCategory = await context.Categories.FirstOrDefaultAsync(c => c.CategoryName == "Software" && c.UserId == null);

            var adobeProvider = await context.Providers.FirstOrDefaultAsync(p => p.ServiceName == "Adobe Creative Cloud" && p.UserId == null);

            if (softwareCategory == null || adobeProvider == null)
            {
                throw new InvalidOperationException("Required seed category or provider was not found");
            }

            var adobeSubscriptionExists = await context.Subscriptions.AnyAsync(s => s.UserId == demoUser.Id && s.ProviderId == adobeProvider.Id);

            if (!adobeSubscriptionExists)
            {

                var newAdobeSubscription = new Subscription
                {
                    UserId = demoUser.Id,
                    CategoryId = softwareCategory.Id,
                    ProviderId = adobeProvider.Id,
                    Price = 669m,
                    BillingInterval = BillingInterval.Monthly,
                    StartDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(-4)),
                    EndDate = null

                };

                await context.Subscriptions.AddAsync(newAdobeSubscription);
            }

            var streamingEndedCategory = await context.Categories.FirstOrDefaultAsync(c => c.CategoryName == "Streaming" && c.UserId == null);

            var disneyProvider = await context.Providers.FirstOrDefaultAsync(p => p.ServiceName == "Disney+" && p.UserId == null);

            if (streamingEndedCategory == null || disneyProvider == null)
            {
                throw new InvalidOperationException("Required seed category or provider was not found");
            }

            var disneySubscriptionExists = await context.Subscriptions.AnyAsync(s => s.UserId == demoUser.Id && s.ProviderId == disneyProvider.Id);

            if (!disneySubscriptionExists)
            {

                var newDisneySubscription = new Subscription
                {
                    UserId = demoUser.Id,
                    CategoryId = streamingEndedCategory.Id,
                    ProviderId = disneyProvider.Id,
                    Price = 109m,
                    BillingInterval = BillingInterval.Monthly,
                    StartDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(-7)),
                    EndDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(-1))

                };

                await context.Subscriptions.AddAsync(newDisneySubscription);
            }


            var productivityCategory = await context.Categories.FirstOrDefaultAsync(c => c.CategoryName == "Productivity" && c.UserId == null);

            var notionProvider = await context.Providers.FirstOrDefaultAsync(p => p.ServiceName == "Notion" && p.UserId == null);

            if (productivityCategory == null || notionProvider == null)
            {
                throw new InvalidOperationException("Required seed category or provider was not found");
            }

            var notionSubscriptionExists = await context.Subscriptions.AnyAsync(s => s.UserId == demoUser.Id && s.ProviderId == notionProvider.Id);

            if (!notionSubscriptionExists)
            {

                var newNotionSubscription = new Subscription
                {
                    UserId = demoUser.Id,
                    CategoryId = productivityCategory.Id,
                    ProviderId = notionProvider.Id,
                    Price = 89m,
                    BillingInterval = BillingInterval.Monthly,
                    StartDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(1)),
                    EndDate = null

                };

                await context.Subscriptions.AddAsync(newNotionSubscription);
            }


            await context.SaveChangesAsync();
        }

    }
}

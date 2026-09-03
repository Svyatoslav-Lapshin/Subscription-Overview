using Microsoft.EntityFrameworkCore;
using SubscriptionOverview.Api.Data;
using SubscriptionOverview.Api.Models;

namespace SubscriptionOverview.Api.Repositories.SubscriptionRepositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly SubscriptionOverviewDbContext _context;

        public SubscriptionRepository(SubscriptionOverviewDbContext context)
        {
            _context = context;
        }

        public async Task AddSubscriptionAsync(Subscription subscription)
        {
            await _context.Subscriptions.AddAsync(subscription);
        }

        public void DeleteSubscription(Subscription subscription)
        {
           _context.Subscriptions.Remove(subscription);
        }

        public async Task<IEnumerable<Subscription>> GetAllSubscriptionsAsync(string userId)
        {
            var subscriptions = await _context.Subscriptions
                                              .Include(c => c.Category)
                                              .Include(p => p.Provider)
                                              .AsNoTracking()
                                              .Where(s => s.UserId == userId)
                                              .ToListAsync();
            return subscriptions;
        }

        public async Task<Subscription?> GetSubscriptionByIdAsync(int id, string userId)
        {
            var subscription = await _context.Subscriptions
                                             .Include(c => c.Category)
                                             .Include(p => p.Provider)
                                             .AsNoTracking()
                                             .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);
            return subscription;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void UpdateSubscription(Subscription subscription)
        {
            _context.Subscriptions.Update(subscription);
        }
    }
}

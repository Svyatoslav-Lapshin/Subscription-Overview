using SubscriptionOverview.Api.Models;

namespace SubscriptionOverview.Api.Repositories.SubscriptionRepositories
{
    public interface ISubscriptionRepository
    {

        Task<IEnumerable<Subscription>> GetAllSubscriptionsAsync(string userId);
        Task<Subscription?> GetSubscriptionByIdAsync(int id, string userId);
        Task AddSubscriptionAsync(Subscription subscription);
        void UpdateSubscription(Subscription subscription);
        void DeleteSubscription(Subscription subscription);    
        Task<bool> SaveChangesAsync();
    }
}


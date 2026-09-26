using SubscriptionOverview.Api.Models;

namespace SubscriptionOverview.Api.Repositories.ProviderRepositories
{
    public interface IProviderRepository
    {

        Task<IEnumerable<Provider>> GetAllProvidersAsync(string userId);
        Task<Provider?> GetProviderByIdAsync(int id, string userId);
        Task<Provider?> GetCustomProviderByIdAsync(int id, string userId);
        Task AddProviderAsync( Provider provider);
        void UpdateProvider(Provider provider);
        void DeleteProvider(Provider provider);
        Task<bool> ExistsByNameAsync(string name, string userId);
        Task<bool> SaveChangesAsync();
    }
}

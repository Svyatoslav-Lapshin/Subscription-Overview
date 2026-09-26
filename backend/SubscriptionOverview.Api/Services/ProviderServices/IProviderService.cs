using SubscriptionOverview.Api.DTOs.ProviderDto;

namespace SubscriptionOverview.Api.Services.ProviderServices
{
    public interface IProviderService
    {
        Task<IEnumerable<ProviderDto>> GetAllProvidersAsync(string userId);
        Task<ProviderDto> GetProviderByIdAsync(int id, string userId);
        Task<ProviderDto> CreateProviderAsync(string userId, ProviderRequestDto providerRequest);
        Task<ProviderDto> UpdateProviderAsync(int id, string userId, ProviderRequestDto providerRequest);
        Task<bool> DeleteProviderAsync(int id, string userId);

    }
}

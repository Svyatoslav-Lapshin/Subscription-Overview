using SubscriptionOverview.Api.DTOs.SubscriptionsDto;

namespace SubscriptionOverview.Api.Services.SubscriptionServices
{
    public interface ISubscriptionService
    {
        Task<IEnumerable<SubscriptionDto>> GetAllSubscriptionsAsync(string userId);
        Task<SubscriptionDto> GetSubscriptionByIdAsync(int id, string userId);
        Task<SubscriptionDto> AddAsync(string userId, SubscriptionRequestDto subscriptionDto);
        Task<SubscriptionDto> UpdateAsync(int id, string userId, SubscriptionRequestDto subscriptionDto);
        Task<bool> DeleteAsync(int id, string userId);

        Task<SubscriptionSummaryDto> GetSummaryAsync(string userId);   

    }
}

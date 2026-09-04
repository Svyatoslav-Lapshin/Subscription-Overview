using SubscriptionOverview.Api.Models;

namespace SubscriptionOverview.Api.Repositories.RefreshTokenRepositories
{
    public interface IRefreshTokenRepository
    {

        Task<RefreshToken?> GetByTokenHashAsync(string tokenHash);
        Task AddAsync(RefreshToken refreshToken);
        Task<bool> SaveChangesAsync();



    }
}

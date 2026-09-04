using SubscriptionOverview.Api.Data;
using SubscriptionOverview.Api.Models;
using Microsoft.EntityFrameworkCore;


namespace SubscriptionOverview.Api.Repositories.RefreshTokenRepositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly SubscriptionOverviewDbContext _context;
        public RefreshTokenRepository(SubscriptionOverviewDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(RefreshToken refreshToken)
        {
            await _context.RefreshTokens.AddAsync(refreshToken);
        }

        public async Task<RefreshToken?> GetByTokenHashAsync(string tokenHash)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.TokenHash == tokenHash);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}

using SubscriptionOverview.Api.Data;
using SubscriptionOverview.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace SubscriptionOverview.Api.Repositories.ProviderRepositories
{
    public class ProviderRepository : IProviderRepository
    {
        private readonly SubscriptionOverviewDbContext _context;

        public ProviderRepository(SubscriptionOverviewDbContext context)
        {
            _context = context;
        }

        public async Task AddProviderAsync(Provider provider)
        {
            await _context.Providers.AddAsync(provider);
        }

        public void DeleteProvider(Provider provider)
        {
            _context.Providers.Remove(provider);
        }

        public async Task<bool> ExistsByNameAsync(string name, string userId)
        {
            return await _context.Providers.AnyAsync(p => p.ServiceName == name && ((p.UserId == null && !p.IsCustom) || (p.UserId == userId && p.IsCustom)));
        }

        public async Task<IEnumerable<Provider>> GetAllProvidersAsync(string userId)  
        {
            return await _context.Providers.AsNoTracking().Where(p =>(p.UserId==null && !p.IsCustom) || (p.UserId == userId && p.IsCustom) ).ToListAsync();

        }
        public async Task<Provider?> GetCustomProviderByIdAsync(int id, string userId)
        {


            return await _context.Providers.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId && p.IsCustom);
        }
        public async Task<Provider?> GetProviderByIdAsync(int id, string userId)
        {
            return await _context.Providers.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id && ((p.UserId == null && !p.IsCustom) || (p.UserId == userId && p.IsCustom)));
        }
        
        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void UpdateProvider(Provider provider)
        {   
            _context.Providers.Update(provider);
        }
    }
}

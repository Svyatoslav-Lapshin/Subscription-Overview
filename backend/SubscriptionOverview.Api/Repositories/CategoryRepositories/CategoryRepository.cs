using SubscriptionOverview.Api.Data;
using SubscriptionOverview.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace SubscriptionOverview.Api.Repositories.CategoryRepositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly SubscriptionOverviewDbContext _context;
        public CategoryRepository(SubscriptionOverviewDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
        }

        public void Delete(Category category)
        {
            _context.Categories.Remove(category);
        }

        public async Task<bool> ExistsByNameAsync(string name, string userId)
        {
            return await _context.Categories.AnyAsync(c => c.CategoryName == name && c.UserId == userId);
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync(string userId)
        {
            return await _context.Categories.AsNoTracking().Where(c => c.UserId == userId).ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id, string userId)
        {
            return await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(Category category)
        {
            _context.Categories.Update(category);
        }
    }
}

using SubscriptionOverview.Api.Models;

namespace SubscriptionOverview.Api.Repositories.CategoryRepositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync(string userId);
        Task<Category?> GetCategoryByIdAsync(int id, string userId);
        Task AddAsync(Category category);
        void Update(Category category);
        void Delete(Category category);
        Task<bool> ExistsByNameAsync(string name, string userId);
        Task<bool> SaveChangesAsync();




    }
}

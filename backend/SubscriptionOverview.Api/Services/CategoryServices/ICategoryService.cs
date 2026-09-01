using SubscriptionOverview.Api.DTOs.CategoriesDto;

namespace SubscriptionOverview.Api.Services.CategoryServices
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync(string userId);
        Task<CategoryDto> GetCategoryByIdAsync(int id, string userId);
        Task<CategoryDto> AddAsync(string userId, CategoryRequestDto categoryDto);
        Task<CategoryDto> UpdateAsync(int id, string userId, CategoryRequestDto categoryDto);
        Task<bool> DeleteAsync(int id, string userId);



    }
}

using SubscriptionOverview.Api.DTOs.CategoriesDto;
using SubscriptionOverview.Api.Models;
using SubscriptionOverview.Api.Repositories.CategoryRepositories;
using SubscriptionOverview.Api.Exceptions;

namespace SubscriptionOverview.Api.Services.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryDto> AddAsync(string userId, CategoryRequestDto categoryDto)
        {
            var existingCategory = await _categoryRepository.ExistsByNameAsync(categoryDto.CategoryName, userId);

            if (existingCategory)
            {
                throw new ConflictException("Category with the same name already exists");
            }

            var category = new Category
            {
                CategoryName = categoryDto.CategoryName,
                UserId = userId
            };

            await _categoryRepository.AddAsync(category);
            var result = await _categoryRepository.SaveChangesAsync();

            if (!result)
            {
                throw new InvalidOperationException("Failed to save category");
            }

            return MapToCategoryDto(category);
        }

        public async Task<bool> DeleteAsync(int id, string userId)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id, userId);

            if (category == null)
            {
                throw new NotFoundException("Category not found");
            }

            _categoryRepository.Delete(category);

            var result = await _categoryRepository.SaveChangesAsync();

            if (!result)
            {
                throw new InvalidOperationException("Failed to delete category");
            }

            return result;

        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync(string userId)
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync(userId);
            var categoryDtos = categories.Select(MapToCategoryDto);
            return categoryDtos;
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(int id, string userId)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id, userId);

            if (category == null)
            {
                throw new NotFoundException("Category not found");

            }

            return MapToCategoryDto(category);
        }

        public async Task<CategoryDto> UpdateAsync(int id, string userId, CategoryRequestDto categoryDto)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id, userId);

            if (category == null)
            {
                throw new NotFoundException("Category not found");
            }

            if (string.Equals(category.CategoryName, categoryDto.CategoryName, StringComparison.OrdinalIgnoreCase))

            {
                return MapToCategoryDto(category);

            }

            var existingCategory = await _categoryRepository.ExistsByNameAsync(categoryDto.CategoryName, userId);

            if (existingCategory)
            {
                throw new ConflictException("Category with the same name already exists");
            }

            category.CategoryName = categoryDto.CategoryName;
            _categoryRepository.Update(category);
            var result = await _categoryRepository.SaveChangesAsync();

            if (!result)
            {
                throw new InvalidOperationException("Failed to save category");
            }
            return MapToCategoryDto(category);

        }
        private static CategoryDto MapToCategoryDto(Category category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                CategoryName = category.CategoryName
            };
        }
    }
}

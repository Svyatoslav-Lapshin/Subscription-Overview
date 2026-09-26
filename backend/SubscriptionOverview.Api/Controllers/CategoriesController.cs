using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubscriptionOverview.Api.DTOs.CategoriesDto;
using SubscriptionOverview.Api.Exceptions;
using SubscriptionOverview.Api.Services.CategoryServices;
using System.Security.Claims;

namespace SubscriptionOverview.Api.Controllers
{
    [ApiController]
    [Route("api/categories")]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllCategories()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                throw new UnauthorizedException("User not authenticated.");
            }
            var categories = await _categoryService.GetAllCategoriesAsync(userId);
            return Ok(categories);

        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoryDto>> GetCategory(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                throw new UnauthorizedException("User not authenticated.");
            }
            var category = await _categoryService.GetCategoryByIdAsync(id, userId);
            
            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDto>> AddCategory([FromBody] CategoryRequestDto categoryDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                throw new UnauthorizedException("User not authenticated.");
            }
            
            var category = await _categoryService.AddAsync(userId, categoryDto);
            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<CategoryDto>> UpdateCategory(int id, [FromBody] CategoryRequestDto categoryDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                throw new UnauthorizedException("User not authenticated.");
            }
            var category = await _categoryService.UpdateAsync(id, userId, categoryDto);
            return Ok(category);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                throw new UnauthorizedException("User not authenticated.");
            }
            await _categoryService.DeleteAsync(id,userId);
            return NoContent();
        }   
    }
}

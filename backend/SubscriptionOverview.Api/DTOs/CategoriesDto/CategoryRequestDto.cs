
using System.ComponentModel.DataAnnotations;

namespace SubscriptionOverview.Api.DTOs.CategoriesDto
{
    public class CategoryRequestDto
    {
       
        [Required(ErrorMessage = "Category name is required")]
        [MaxLength(100, ErrorMessage = "Category name cannot exceed 100 characters")]
        public string CategoryName { get; set; } = string.Empty;
       


    }
}

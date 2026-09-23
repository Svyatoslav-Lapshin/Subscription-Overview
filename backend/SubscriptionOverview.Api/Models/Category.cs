using Microsoft.EntityFrameworkCore;
using SubscriptionOverview.Api.Models.Identity;
using System.ComponentModel.DataAnnotations;

namespace SubscriptionOverview.Api.Models
{

    public class Category
    {
        public int Id { get; set; }

        //Name of the subscription category, e.g. Entertaiment or Software.
        [Required(ErrorMessage = "Category name is required")]
        [MaxLength(100, ErrorMessage = "Category name cannot exceed 100 characters")]
        public string CategoryName { get; set; } = string.Empty;

        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; } 

        //Subscriptions associated with this category.
        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();

    }
}

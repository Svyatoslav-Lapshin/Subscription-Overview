using Microsoft.EntityFrameworkCore;
using SubscriptionOverview.Api.Models.Identity;
using System.ComponentModel.DataAnnotations;

namespace SubscriptionOverview.Api.Models
{
    [Index(nameof(ServiceName))]
    public class Provider
    {
        public int Id { get; set; }
        
        //Name of the subscription service, e.g. Netflix or Spotify
        [Required(ErrorMessage = "Service name is required")]
        [MaxLength(100, ErrorMessage = "Service name cannot exceed 100 characters")]
        public string ServiceName { get; set; } = string.Empty;

        // Nullable user because global providers are not connected to a specific user.
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; } 

        //Indicates wheter the provider was created by a user or is a predefined provider.
        public bool IsCustom { get; set; }

        //Subscriptions associated with this provider.
        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();

    }
}

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SubscriptionOverview.Api.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;


        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
        public ICollection<Category> Categories { get; set; } = new List<Category>();
        public ICollection<Provider> CustomProviders { get; set; } = new List<Provider>();


    }
}

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


        public ICollection<Subscription> Subscriptions { get; set; } = new HashSet<Subscription>();
        public ICollection<Category> Categories { get; set; } = new HashSet<Category>();
        public ICollection<Provider> CustomProviders { get; set; } = new HashSet<Provider>();



    }
}

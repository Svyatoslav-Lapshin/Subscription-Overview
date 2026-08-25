
using Microsoft.EntityFrameworkCore;
using SubscriptionOverview.Api.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace SubscriptionOverview.Api.Models
{
    [Index(nameof(Price))]
    [Index(nameof(StartDate))]
    public class Subscription
    {
        public int Id { get; set; }

        //Validates the monetory value using decimal boundaries.
        [Range(typeof(decimal), "0.01", "999999.99", ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        //Validates that the value belongs to the BillingInterval.
        [EnumDataType(typeof(BillingInterval), ErrorMessage = "Please select a valid billing interval.")]
        public BillingInterval BillingInterval { get; set; }

        // Foreign key for the selected category.
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid category.")]
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        // Foreign key for the selected provider.
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid provider.")]
        public int ProviderId { get; set; }
        public Provider Provider { get; set; } = null!;

        // The date when the subscription starts.
        public DateOnly StartDate { get; set; }
        // Null means the subscription is still active.
        public DateOnly? EndDate { get; set; }

    }
}

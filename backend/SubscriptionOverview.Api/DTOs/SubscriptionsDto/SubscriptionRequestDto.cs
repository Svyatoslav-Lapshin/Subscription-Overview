using SubscriptionOverview.Api.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace SubscriptionOverview.Api.DTOs.SubscriptionsDto
{
    public class SubscriptionRequestDto
    {
        //ParseLimitsInInvariantCulture ensures that the decimal values are parsed correctly regardless of the culture settings.
        [Range(typeof(decimal), "0.01", "999999.99", ErrorMessage = "Price must be greater than 0.", ParseLimitsInInvariantCulture = true)]
        public decimal Price { get; set; }
        //Validates that the value belongs to the BillingInterval.
        [EnumDataType(typeof(BillingInterval), ErrorMessage = "Please select a valid billing interval.")]
        public BillingInterval BillingInterval { get; set; }
        // Foreign key for the selected category.
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid category.")]
        public int CategoryId { get; set; }
        // Foreign key for the selected provider.
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid provider.")]
        public int ProviderId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
    }
}

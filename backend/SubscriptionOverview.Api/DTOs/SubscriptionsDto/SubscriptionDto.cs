using SubscriptionOverview.Api.Models.Enums;

namespace SubscriptionOverview.Api.DTOs.SubscriptionsDto
{
    public class SubscriptionDto
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public BillingInterval BillingInterval { get; set; }
        public decimal MonthlyCost { get; set; }
        public decimal YearlyCost{ get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int ProviderId { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }

    }
}

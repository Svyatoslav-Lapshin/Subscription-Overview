namespace SubscriptionOverview.Api.DTOs.SubscriptionsDto
{
    public class SubscriptionSummaryDto
    {
        public decimal TotalMonthlyPayments { get; set; }
        public decimal TotalYearlyPayments { get; set; }
        public int ActiveSubscriptionsCount { get; set; }
        public int TotalSubscriptionsCount { get; set; }

    }
}

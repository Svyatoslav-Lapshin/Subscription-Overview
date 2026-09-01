namespace SubscriptionOverview.Api.DTOs.ProviderDto
{
    public class ProviderDto
    {
        public int Id { get; set; }

        public string ServiceName { get; set; } = string.Empty;

        public bool IsCustom { get; set; }
    }
}

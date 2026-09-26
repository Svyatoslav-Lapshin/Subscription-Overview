namespace SubscriptionOverview.Api.Services.Auth
{
    public class TokenResult
    {
        public string AccessToken { get; set; } = string.Empty;
        public DateTimeOffset ExpiresAt { get; set; }


    }
}

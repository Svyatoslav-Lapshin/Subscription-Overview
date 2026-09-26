using SubscriptionOverview.Api.DTOs.Auth;

namespace SubscriptionOverview.Api.Services.Auth
{
    public class AuthResult
    {
        public AuthResponseDto Response { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public DateTime RefreshTokenExpiresAt { get; set; }

    }
}

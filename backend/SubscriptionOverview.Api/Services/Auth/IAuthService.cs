using SubscriptionOverview.Api.DTOs.Auth;

namespace SubscriptionOverview.Api.Services.Auth
{
    public interface IAuthService
    {

        Task<AuthResult> RegisterAsync(RegisterDto dto);
        Task<AuthResult> LoginAsync(LoginDto dto);

        Task<AuthResult> RefreshTokenAsync(string refreshToken);
        Task LogoutAsync(string refreshToken);
    }
}

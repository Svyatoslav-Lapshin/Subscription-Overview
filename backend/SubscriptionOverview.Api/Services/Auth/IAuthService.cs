using SubscriptionOverview.Api.DTOs.Auth;

namespace SubscriptionOverview.Api.Services.Auth
{
    public interface IAuthService
    {

        Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
    }
}

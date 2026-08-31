using SubscriptionOverview.Api.Models.Identity;

namespace SubscriptionOverview.Api.Services.Auth
{
    public interface ITokenService
    {

        TokenResult CreateToken(ApplicationUser user);
    }
}

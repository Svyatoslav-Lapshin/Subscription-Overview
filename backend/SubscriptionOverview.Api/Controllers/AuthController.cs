using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubscriptionOverview.Api.DTOs.Auth;
using SubscriptionOverview.Api.Exceptions;
using SubscriptionOverview.Api.Services.Auth;

namespace SubscriptionOverview.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto registerDto)
        {

            var result = await _authService.RegisterAsync(registerDto);
            SetRefreshTokenCookie(result.RefreshToken, result.RefreshTokenExpiresAt);
            return Ok(result.Response);

        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
        {

            var result = await _authService.LoginAsync(loginDto);

            SetRefreshTokenCookie(result.RefreshToken, result.RefreshTokenExpiresAt);
            return Ok(result.Response);

        }

        [HttpPost("logout")]
        [AllowAnonymous]
        public async Task<ActionResult> Logout()
        {


            var refreshToken = Request.Cookies["refreshToken"];

            if (!string.IsNullOrWhiteSpace(refreshToken))
            {

                await _authService.LogoutAsync(refreshToken);
            }
      
            Response.Cookies.Delete("refreshToken", new CookieOptions { Path = "/api/auth" });

            return NoContent();

        }
        // Refresh token endpoint need to user if the access token is expired and the refresh token is still valid.
        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponseDto>> Refresh()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                throw new UnauthorizedException("Refresh token is missing or invalid.");
            }

            var result = await _authService.RefreshTokenAsync(refreshToken);

            SetRefreshTokenCookie(result.RefreshToken, result.RefreshTokenExpiresAt);
            return Ok(result.Response);

        }

        private void SetRefreshTokenCookie(string refreshToken, DateTime expiresAt)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expiresAt,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Path= "/api/auth"
            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubscriptionOverview.Api.DTOs.Auth;
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

            return Ok(result);

        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
        {

            var result = await _authService.LoginAsync(loginDto);

            return Ok(result);

        }


    }
}

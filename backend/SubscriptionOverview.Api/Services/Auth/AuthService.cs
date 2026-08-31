using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using SubscriptionOverview.Api.DTOs.Auth;
using SubscriptionOverview.Api.Exceptions;
using SubscriptionOverview.Api.Models.Identity;

namespace SubscriptionOverview.Api.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthService(UserManager<ApplicationUser> userManager, ITokenService tokenService, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }


        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user is null)
            {

                throw new UnauthorizedException("Invalid email or password");
            }

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, lockoutOnFailure: true);

            if (signInResult.IsLockedOut)
            {

                throw new UnauthorizedException("Invalid email or password");
            }

            if (!signInResult.Succeeded)
            {

                throw new UnauthorizedException("Invalid email or password");
            }

            var token = _tokenService.CreateToken(user);
            var authResponseDto = new AuthResponseDto
            {
                AccessToken = token.AccessToken,
                Email = user.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ExpiresAt = token.ExpiresAt
            };

            return authResponseDto;


        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);

            if (existingUser is not null)
            {
              
                throw new ConflictException("Email already exists.");
            }

            var user = new ApplicationUser
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                UserName = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(error => error.Description));

                throw new AppValidationException(errors);
            }

            var token = _tokenService.CreateToken(user);

            var authResponseDto = new AuthResponseDto
            {
                AccessToken = token.AccessToken,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ExpiresAt = token.ExpiresAt
            };

            return authResponseDto;


        }
    }
}

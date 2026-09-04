using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using SubscriptionOverview.Api.DTOs.Auth;
using SubscriptionOverview.Api.Exceptions;
using SubscriptionOverview.Api.Models;
using SubscriptionOverview.Api.Models.Identity;
using SubscriptionOverview.Api.Repositories.RefreshTokenRepositories;

namespace SubscriptionOverview.Api.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public AuthService(UserManager<ApplicationUser> userManager, ITokenService tokenService, SignInManager<ApplicationUser> signInManager, IRefreshTokenRepository refreshTokenRepository)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _refreshTokenRepository = refreshTokenRepository;
        }


        public async Task<AuthResult> LoginAsync(LoginDto dto)
        {
            //Find the user by email using UserManager.
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user is null)
            {
                throw new UnauthorizedException("Invalid email or password");
            }

            //Check the password using SignInManager.
            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, lockoutOnFailure: true);

            //If the user is locked out, throw an UnauthorizedException.
            if (signInResult.IsLockedOut)
            {
                throw new UnauthorizedException("Invalid email or password");
            }

            //If the sign-in attempt failed, throw an UnauthorizedException.
            if (!signInResult.Succeeded)
            {
                throw new UnauthorizedException("Invalid email or password");
            }

            //Generate an access token for the user using TokenService.
            var token = _tokenService.CreateToken(user);

            //Generate a refresh token for the user using TokenService.
            var refreshToken = _tokenService.GenerateRefreshToken();

            //Hash the refresh token for secure storage using TokenService.
            var tokenHash = _tokenService.HashRefreshToken(refreshToken);

            //Create an AuthResponseDto to return the access token and user information.
            var authResponseDto = new AuthResponseDto
            {
                AccessToken = token.AccessToken,
                Email = user.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ExpiresAt = token.ExpiresAt
            };

            //Create a new RefreshToken entity to store the hashed refresh token and its expiration date.
            var refreshTokenEntity = new RefreshToken
            {
                TokenHash = tokenHash,
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(7), // Set the expiration date for the refresh token.
            };

            // Save the new refresh token entity to the database.
            await _refreshTokenRepository.AddAsync(refreshTokenEntity);

            // Save the changes to the database.
            var result = await _refreshTokenRepository.SaveChangesAsync();

            // Check if the save operation was successful.
            if (!result)
            {
                throw new InvalidOperationException("Failed to save refresh token.");
            }

            // Create the AuthResult with the access token, refresh token, and expiration date.
            var authResult = new AuthResult
            {
                Response = authResponseDto,
                RefreshToken = refreshToken,
                RefreshTokenExpiresAt = refreshTokenEntity.ExpiresAt
            };
            // Return the AuthResult to the caller.
            return authResult;

        }

        public async Task LogoutAsync(string refreshToken)
        {
            var tokenHash = _tokenService.HashRefreshToken(refreshToken);

            var searchToken = await _refreshTokenRepository.GetByTokenHashAsync(tokenHash);

            // If the refresh token is not found or already revoked, return without throwing an exception.
            if (searchToken == null || searchToken.RevokedAt != null)
            {
                return; 
            }

            // Revoke the current refresh token.
            searchToken.RevokedAt = DateTime.UtcNow;

            // Save the changes to the database.
            var result = await _refreshTokenRepository.SaveChangesAsync();
            if (!result)
            {
                throw new InvalidOperationException("Failed to revoke refresh token.");
            }
        
        }

        public async Task<AuthResult> RefreshTokenAsync(string refreshToken)
        {
            // Validate the refresh token.
            var tokenHash = _tokenService.HashRefreshToken(refreshToken);
            // Retrieve the refresh token from the database.
            var searchToken = await _refreshTokenRepository.GetByTokenHashAsync(tokenHash);

            var now = DateTime.UtcNow;

            // Check if the refresh token is valid and not expired or revoked.
            if (searchToken == null || searchToken.ExpiresAt <= now || searchToken.RevokedAt != null)
            {
                throw new UnauthorizedException("Invalid refresh token.");

            }
            // Retrieve the user associated with the refresh token.
            var user = await _userManager.FindByIdAsync(searchToken.UserId);

            if (user == null)
            {
                throw new UnauthorizedException("Invalid refresh token.");
            }
            // Revoke the old refresh token and generate a new one.
            searchToken.RevokedAt = now;
            
            // Generate a new refresh token.
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            // Hash the new refresh token.
            var newHashRefreshToken = _tokenService.HashRefreshToken(newRefreshToken);

            // Set the new refresh token hash in the old refresh token entity.
            searchToken.ReplacedByTokenHash = newHashRefreshToken;

            // Create a new refresh token entity and save it to the database.
            var newRefreshTokenEntity = new RefreshToken
            {
                TokenHash = newHashRefreshToken,
                UserId = user.Id,
                ExpiresAt = now.AddDays(7), // Set the expiration date for the new refresh token.
            };

            // Save the new refresh token entity to the database.
            await _refreshTokenRepository.AddAsync(newRefreshTokenEntity);

            // Generate a new access token for the user
            var newAccessToken = _tokenService.CreateToken(user);

            // Create the AuthResponseDto with the new access token and user information.
            var authResponseDto = new AuthResponseDto
            {
                AccessToken = newAccessToken.AccessToken,
                Email = user.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ExpiresAt = newAccessToken.ExpiresAt
            };

            // Save the changes to the database.
            var result = await _refreshTokenRepository.SaveChangesAsync();

            // Check if the save operation was successful.
            if (!result)
            {

                throw new InvalidOperationException("Failed to save refresh token.");
            }
            // Create the AuthResult with the new access token, refresh token, and expiration date.
            var authResult = new AuthResult
            {
                Response = authResponseDto,
                RefreshToken = newRefreshToken,
                RefreshTokenExpiresAt = newRefreshTokenEntity.ExpiresAt

            };

            // Return the AuthResult.
            return authResult;
        }

        public async Task<AuthResult> RegisterAsync(RegisterDto dto)
        {
            // Check if the email already exists.
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);

            if (existingUser is not null)
            {

                throw new ConflictException("Email already exists.");
            }
            // Create a new ApplicationUser instance with the provided registration details.
            var user = new ApplicationUser
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                UserName = dto.Email
            };
            // Create the user using UserManager and handle any errors that may occur.
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(error => error.Description));

                throw new AppValidationException(errors);
            }
            // Generate an access token for the newly registered user.
            var token = _tokenService.CreateToken(user);
            // Generate a refresh token and hash it for storage.
            var refreshToken = _tokenService.GenerateRefreshToken();
            // Hash the refresh token for secure storage.
            var tokenHash = _tokenService.HashRefreshToken(refreshToken);
            // Create an AuthResponseDto to return the access token and user information.
            var authResponseDto = new AuthResponseDto
            {
                AccessToken = token.AccessToken,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ExpiresAt = token.ExpiresAt
            };
            // Create a new RefreshToken entity to store the hashed refresh token and its expiration date.
            var refreshTokenEntity = new RefreshToken
            {
                TokenHash = tokenHash,
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(7), // Set the expiration date for the refresh token.
            };
            // Save the new refresh token entity to the database.
            await _refreshTokenRepository.AddAsync(refreshTokenEntity);
            var saved = await _refreshTokenRepository.SaveChangesAsync();
            // Check if the save operation was successful.
            if (!saved)
            {
                throw new InvalidOperationException("Failed to save refresh token.");
            }
            // Create the AuthResult with the access token, refresh token, and expiration date.
            var authResult = new AuthResult
            {
                Response = authResponseDto,
                RefreshToken = refreshToken,
                RefreshTokenExpiresAt = refreshTokenEntity.ExpiresAt

            };

            // Return the AuthResult to the caller.
            return authResult;


        }

    }
}

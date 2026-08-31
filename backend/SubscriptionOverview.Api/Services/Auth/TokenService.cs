using Microsoft.IdentityModel.Tokens;
using SubscriptionOverview.Api.Models.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SubscriptionOverview.Api.Services.Auth
{
    public class TokenService : ITokenService
    {

        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public TokenResult CreateToken(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Email,user.Email!),
                new Claim("firstName", user.FirstName),
                new Claim("lastName", user.LastName)


            };

            var secretKey = _configuration["JWT:SigningKey"];
            var issuer = _configuration["JWT:Issuer"];
            var audience = _configuration["JWT:Audience"];
            var expirationMinutes = _configuration["JWT:ExpirationMinutes"];


            if (!int.TryParse(expirationMinutes, out var expiryMinutes) || expiryMinutes <= 0)
            {
                throw new InvalidOperationException("JWT expiration must be a positive number.");
            }

            var expiresAt = DateTimeOffset.UtcNow.AddMinutes(expiryMinutes);

            if (string.IsNullOrWhiteSpace(secretKey))
            {
                throw new InvalidOperationException("JWT signing key is not configured.");
            }


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenSecurity = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiresAt.UtcDateTime,
                signingCredentials: credentials

                );

            var tokenHandler = new JwtSecurityTokenHandler();

            var accessToken = tokenHandler.WriteToken(tokenSecurity);


            return new TokenResult { 
                AccessToken = accessToken,
                ExpiresAt = expiresAt };

        }
    }
}

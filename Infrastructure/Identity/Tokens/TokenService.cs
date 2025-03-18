using Application.Exceptions;
using Application.Features.Identity.Tokens;
using Infrastructure.Identity.Auth.Jwt;
using Infrastructure.Identity.Constants;
using Infrastructure.Identity.Models;
using Infrastructure.Tenancy;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Identity.Tokens
{
    public class TokenService(UserManager<ApplicationUser> userManager, EduTenantInfo tenant, IOptions<JwtSettings> jwtSettings) : ITokenService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly EduTenantInfo _tenant = tenant;
        private readonly JwtSettings _jwtSettings = jwtSettings.Value;

        public async Task<TokenResponse> LoginAsync(TokenRequest request)
        {
            var userInDb = await _userManager.FindByEmailAsync(request.Email) ?? throw new UnAuthorizedException("Authentication Invalid!!");
            if(!await _userManager.CheckPasswordAsync(userInDb, request.Password))
            {
                throw new UnAuthorizedException("Incorrect Username or Password.");
            }

            if (!userInDb.IsActive)
            {
                throw new UnAuthorizedException("User Not Active. Please contact to admin.");
            }

            if(_tenant.Id != TenancyConstants.Root.Id)
            {
                if(_tenant.ValidUpTo < DateTime.UtcNow)
                {
                    throw new UnAuthorizedException("Tenant subscription has expired. Please contact admin");
                }
            }

            return await GenerateTokenAndUpdateUserAsync(userInDb);
        }

        public async Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var userPrinciple = GetClaimsPrincipalFromExpiredToken(request.CurrentJwtToken);
            var userEmail = userPrinciple.GetEmail();

            var userInDb = await _userManager.FindByEmailAsync(userEmail) 
                ?? throw new UnAuthorizedException("Authentication Invalid!!");

            return await GenerateTokenAndUpdateUserAsync(userInDb);
        }

        private ClaimsPrincipal GetClaimsPrincipalFromExpiredToken(string expiredToken)
        {
            var tokenValidationParams = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero,
                RoleClaimType = ClaimTypes.Role,
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principle = tokenHandler.ValidateToken(expiredToken, tokenValidationParams, out var securityToken);

            if(securityToken is not JwtSecurityToken jwtSecurityToken
                || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new UnAuthorizedException("Invalid token. Failed to create refresh token");
            }

            return principle;
        }

        private async Task<TokenResponse> GenerateTokenAndUpdateUserAsync(ApplicationUser user)
        {
            string newToken = GenerateJwt(user);

            user.RefreshToken = GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.TokenExpiryTimeInDays);

            await _userManager.UpdateAsync(user);

            return new()
            {
                JwtToken = newToken,
                RefreshToken = user.RefreshToken,
                RefreshTokenExpiryDate = user.RefreshTokenExpiryTime
            };
        }

        private string GenerateJwt(ApplicationUser user)
        {
            return GenerateEncyptedToken(GetSigningCredentials(), GetUserClaims(user));
        }

        private string GenerateRefreshToken()
        {
            byte[] randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private string GenerateEncyptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
        {
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpiryTimeInMinutes),
                signingCredentials: signingCredentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        private SigningCredentials GetSigningCredentials()
        {
            byte[] secret = Encoding.UTF8.GetBytes(_jwtSettings.Key);
            return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
        }

        private IEnumerable<Claim> GetUserClaims(ApplicationUser user)
        {
            return
            [
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimConstants.Tenant, _tenant.Id),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber)
            ];
        }

    }
}

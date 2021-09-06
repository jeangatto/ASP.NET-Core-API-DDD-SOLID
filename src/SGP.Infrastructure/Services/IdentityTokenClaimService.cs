using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Ardalis.GuardClauses;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SGP.Shared.AppSettings;
using SGP.Shared.Interfaces;
using SGP.Shared.Records;

namespace SGP.Infrastructure.Services
{
    public class IdentityTokenClaimService : ITokenClaimsService
    {
        private const short RefreshTokenBytesLength = 64;
        private readonly JwtConfig _jwtConfig;
        private readonly IDateTime _dateTime;

        public IdentityTokenClaimService(IOptions<JwtConfig> jwtOptions, IDateTime dateTime)
        {
            _jwtConfig = jwtOptions.Value;
            _dateTime = dateTime;
        }

        public AccessToken GenerateAccessToken(Claim[] claims)
        {
            Guard.Against.NullOrEmpty(claims, nameof(claims));

            var createdAt = _dateTime.Now;
            var expiresAt = createdAt.AddSeconds(_jwtConfig.Seconds);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _jwtConfig.Audience,
                Issuer = _jwtConfig.Issuer,
                NotBefore = createdAt,
                Expires = expiresAt,
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = CreateSigningCredentials()
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);
            return new AccessToken(token, createdAt, expiresAt);
        }

        public string GenerateRefreshToken()
        {
            using (var cryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[RefreshTokenBytesLength];
                cryptoServiceProvider.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes);
            }
        }

        private SigningCredentials CreateSigningCredentials()
        {
            var secretKey = Encoding.ASCII.GetBytes(_jwtConfig.Secret);
            var securityKey = new SymmetricSecurityKey(secretKey);
            return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        }
    }
}
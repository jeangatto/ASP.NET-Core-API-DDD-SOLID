namespace SGP.Infrastructure.Services
{
    using Ardalis.GuardClauses;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using Shared.AppSettings;
    using Shared.Extensions;
    using Shared.Interfaces;
    using Shared.Records;
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Text;

    public class IdentityTokenClaimService : ITokenClaimsService
    {
        private readonly JwtConfig _jwtConfig;
        private readonly IDateTime _dateTime;

        public IdentityTokenClaimService(IOptions<JwtConfig> jwtOptions, IDateTime dateTime)
        {
            Guard.Against.NullOptions(jwtOptions, nameof(jwtOptions));

            _jwtConfig = jwtOptions.Value;
            _dateTime = dateTime;
        }

        public AccessToken GenerateAccessToken(IEnumerable<Claim> claims)
        {
            Guard.Against.NullOrEmpty(claims, nameof(claims));

            var createdAt = _dateTime.Now;
            var expiresAt = createdAt.AddSeconds(_jwtConfig.Seconds);
            var secretKey = Encoding.ASCII.GetBytes(_jwtConfig.Secret);
            var securityKey = new SymmetricSecurityKey(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _jwtConfig.Audience,
                Issuer = _jwtConfig.Issuer,
                NotBefore = createdAt,
                Expires = expiresAt,
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
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
                var randomBytes = new byte[64];
                cryptoServiceProvider.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes);
            }
        }
    }
}
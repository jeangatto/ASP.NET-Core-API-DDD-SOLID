using Ardalis.GuardClauses;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SGP.Shared.AppSettings;
using SGP.Shared.Entities;
using SGP.Shared.Extensions;
using SGP.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SGP.Infrastructure.Services
{
    public class JWTokenService : IJWTokenService
    {
        private readonly JwtConfig _jwtConfig;
        private readonly IDateTime _dateTime;

        public JWTokenService(IOptions<JwtConfig> jwtOptions, IDateTime dateTime)
        {
            Guard.Against.Null(jwtOptions);

            _jwtConfig = jwtOptions.Value;
            _dateTime = dateTime;
        }

        public JWToken GenerateToken(IEnumerable<Claim> claims)
        {
            // TODO: verificação dos valores appsettings

            var createdAt = _dateTime.Now;
            var secondsToExpire = _jwtConfig.Seconds;
            var expiresAt = createdAt.AddSeconds(secondsToExpire);
            var secretKey = Encoding.UTF8.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _jwtConfig.Audience,
                Issuer = _jwtConfig.Issuer,
                NotBefore = createdAt,
                Expires = expiresAt,
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);
            var refreshToken = GenerateRefreshToken();

            return new JWToken(
                accessToken,
                refreshToken,
                createdAt,
                expiresAt,
                secondsToExpire);
        }

        private static string GenerateRefreshToken()
        {
            using (var cryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                cryptoServiceProvider.GetBytes(randomBytes);
                var base64 = Convert.ToBase64String(randomBytes);
                return base64.RemoveIlegalCharactersFromURL();
            }
        }
    }
}
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Ardalis.GuardClauses;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SGP.Shared.Abstractions;
using SGP.Shared.AppSettings;
using SGP.Shared.Records;

namespace SGP.Infrastructure.Services;

public class JwtClaimService(IOptions<JwtOptions> jwtOptions, IDateTimeService dateTimeService) : ITokenClaimsService
{
    private const short RefreshTokenBytesLength = 64;
    private readonly IDateTimeService _dateTimeService = dateTimeService;
    private readonly JwtOptions _jwtConfig = jwtOptions.Value;

    public AccessToken GenerateAccessToken(Claim[] claims)
    {
        Guard.Against.NullOrEmpty(claims);

        var createdAt = _dateTimeService.Now;
        var expiresAt = createdAt.AddSeconds(_jwtConfig.Seconds);
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Audience = _jwtConfig.Audience,
            Issuer = _jwtConfig.Issuer,
            NotBefore = createdAt,
            Expires = expiresAt,
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = CreateSigningCredentials()
        });

        var token = tokenHandler.WriteToken(securityToken);
        return new AccessToken(token, createdAt, expiresAt);
    }

    public string GenerateRefreshToken()
    {
        using var rnd = RandomNumberGenerator.Create();
        var randomBytes = new byte[RefreshTokenBytesLength];
        rnd.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    private SigningCredentials CreateSigningCredentials()
    {
        var secretKey = Encoding.ASCII.GetBytes(_jwtConfig.Secret);
        var securityKey = new SymmetricSecurityKey(secretKey);
        return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
    }
}
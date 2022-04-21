using System;
using System.Text;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using SGP.Shared.AppSettings;

namespace SGP.PublicApi.Extensions;

internal static class JwtBearerExtensions
{
    internal static IServiceCollection AddJwtBearer(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        Guard.Against.Null(configuration, nameof(configuration));
        Guard.Against.Null(environment, nameof(environment));

        var jwtConfig = GetJwtConfig(configuration);

        services
            .AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(bearerOptions =>
            {
                // Note: RequireHttpsMetadata deve estar sempre habilitado no ambiente de produção.
                bearerOptions.RequireHttpsMetadata = environment.IsProduction();
                bearerOptions.SaveToken = true;
                bearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfig.Secret)),
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidAudience = jwtConfig.Audience,
                    ValidIssuer = jwtConfig.Issuer
                };
            });

        // Ativa o uso do token como forma de autorizar o acesso a recursos deste projeto.
        services.AddAuthorization(authOptions =>
        {
            authOptions.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build());
        });

        return services;
    }

    private static JwtConfig GetJwtConfig(IConfiguration configuration)
        => configuration
            .GetSection(nameof(JwtConfig))
            .Get<JwtConfig>(options => options.BindNonPublicProperties = true);
}
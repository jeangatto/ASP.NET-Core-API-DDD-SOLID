using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SGP.Shared.AppSettings;
using SGP.Shared.Extensions;
using Throw;

namespace SGP.PublicApi.Extensions;

internal static class JwtBearerExtensions
{
    internal static void AddJwtBearer(this IServiceCollection services, IConfiguration configuration)
    {
        configuration.ThrowIfNull();

        var jwtConfig = configuration.GetWithNonPublicProperties<JwtConfig>();
        jwtConfig.ThrowIfNull().IfNullOrEmpty(x => x.Secret);

        services
            .AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(bearerOptions =>
            {
                bearerOptions.RequireHttpsMetadata = false;
                bearerOptions.SaveToken = true;
                bearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = jwtConfig.ValidateIssuer,
                    ValidateAudience = jwtConfig.ValidateAudience,
                    ValidIssuer = jwtConfig.Issuer,
                    ValidAudience = jwtConfig.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfig.Secret)),

                    // Valida a assinatura de um token recebido.
                    ValidateIssuerSigningKey = true,

                    // Verifica se um token recebido ainda é válido.
                    ValidateLifetime = true,

                    // Tempo de tolerância para a expiração de um token (utilizado
                    // caso haja problemas de sincronismo de horário entre diferentes
                    // computadores envolvidos no processo de comunicação).
                    ClockSkew = TimeSpan.Zero
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
    }
}
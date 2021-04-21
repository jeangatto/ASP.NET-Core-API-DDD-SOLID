using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SGP.Shared.AppSettings;
using System;
using System.Text;

namespace SGP.PublicApi.Extensions
{
    public static class JwtBearerExtensions
    {
        public static IServiceCollection AddJwtBearer(this IServiceCollection services,
            IConfiguration configuration)
        {
            Guard.Against.Null(services, nameof(services));
            Guard.Against.Null(configuration, nameof(configuration));

            var jwtConfig = new JwtConfig();

            configuration.GetSection(nameof(JwtConfig)).Bind(jwtConfig,
                binderOptions => binderOptions.BindNonPublicProperties = true);

            var secretKey = Encoding.ASCII.GetBytes(jwtConfig.Secret);

            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions =>
            {
                bearerOptions.RequireHttpsMetadata = false;
                bearerOptions.SaveToken = true;
                bearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = jwtConfig.Issuer,
                    ValidAudience = jwtConfig.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),

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

            // Ativa o uso do token como forma de autorizar o acesso
            // a recursos deste projeto.
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                    .RequireAuthenticatedUser()
                    .Build());
            });

            return services;
        }
    }
}
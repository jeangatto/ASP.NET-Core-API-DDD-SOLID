using Microsoft.Extensions.Options;
using SGP.Shared.AppSettings;

namespace SGP.Tests.DataFakers
{
    public static class OptionsDataFaker
    {
        private const short MaximumAttempts = 3;
        private const short SecondsBlocked = 1000;
        private const string Audience = "Clients-API-SGP";
        private const string Issuer = "API-SGP";
        private const short Seconds = 21600;
        private const string SecretKey = "p8SXNddEAEn1cCuyfVJKYA7e6hlagbLd";

        public static readonly IOptions<AuthConfig> AuthConfigOptions =
            Options.Create(AuthConfig.Create(MaximumAttempts, SecondsBlocked));

        public static readonly IOptions<JwtConfig> JwtConfigOptions =
            Options.Create(JwtConfig.Create(Audience, Issuer, Seconds, SecretKey, true, true));
    }
}
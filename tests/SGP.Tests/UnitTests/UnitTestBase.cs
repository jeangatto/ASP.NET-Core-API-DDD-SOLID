using Bogus;
using Microsoft.Extensions.Options;
using SGP.Shared.AppSettings;

namespace SGP.Tests.UnitTests
{
    public abstract class UnitTestBase
    {
        protected static readonly Faker Faker = new();

        // AuthConfig
        private const short MaximumAttempts = 3;
        private const short SecondsBlocked = 1000;

        // JwtConfig
        private const string Audience = "Clients-API-SGP";
        private const string Issuer = "API-SGP";
        private const short Seconds = 21600;

        // SecretKey
        private const int SecretKeySize = 32;
        private const string SecretKeyFormat = "123456789.abcdefghijklmnopqrstuvwxyz-ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        protected static IOptions<AuthConfig> CreateAuthConfigOptions()
        {
            return Options.Create(AuthConfig.Create(MaximumAttempts, SecondsBlocked));
        }

        protected static IOptions<JwtConfig> CreateJwtConfigOptions()
        {
            var secret = Faker.Random.String2(SecretKeySize, SecretKeyFormat);
            return Options.Create(JwtConfig.Create(Audience, Issuer, Seconds, secret, true, true));
        }
    }
}
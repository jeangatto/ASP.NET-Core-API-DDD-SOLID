using SGP.Tests.Constants;
using SGP.Tests.Fixtures;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.UnitTests.Application.Services
{
    [UnitTest(TestCategories.Application)]
    public class AuthenticationServiceTests : IClassFixture<EfSqliteFixture>
    {
        private readonly EfSqliteFixture _fixture;

        public AuthenticationServiceTests(EfSqliteFixture fixture)
        {
            _fixture = fixture;
        }
    }
}
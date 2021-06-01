using AutoMapper;
using SGP.Application.Mapper;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.UnitTests.Application
{
    [UnitTest(TestCategories.Application)]
    public class DomainToResponseMapperTests
    {
        [Fact]
        public void Should_Mapper_ConfigurationIsValid()
        {
            // Arrange
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile<DomainToResponseMapper>());

            // Act
            var mapper = new Mapper(configuration);

            // Assert
            mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}

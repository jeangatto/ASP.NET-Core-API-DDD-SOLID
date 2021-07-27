namespace SGP.Tests.UnitTests.Application
{
    using AutoMapper;
    using Constants;
    using SGP.Application.Mapper;
    using Xunit;
    using Xunit.Categories;

    [UnitTest(TestCategories.Application)]
    public class DomainToResponseMapperTests
    {
        [Fact]
        public void Should_Mapper_ConfigurationIsValid()
        {
            // Arrange
            var configuration = new MapperConfiguration(cfg
                => cfg.AddProfile<DomainToResponseMapper>());

            // Act
            var mapper = new Mapper(configuration);

            // Assert
            mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}

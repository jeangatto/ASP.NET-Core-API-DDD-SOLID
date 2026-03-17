using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using SGP.Application.Mapper;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.UnitTests.Application;

[UnitTest]
public class DomainToResponseMapperTests
{
    [Fact]
    public void Should_Mapper_ConfigurationIsValid()
    {
        // Arrange
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile<DomainToResponseMapper>(), new NullLoggerFactory());

        // Act
        var mapper = new Mapper(configuration);

        // Assert
        mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }
}
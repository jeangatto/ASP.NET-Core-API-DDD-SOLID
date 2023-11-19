using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using SGP.Application.Mapper;
using SGP.Application.Services;
using SGP.Infrastructure.Data.Repositories;
using SGP.Tests.Constants;
using SGP.Tests.Extensions;
using SGP.Tests.Fixtures;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.UnitTests.Application.Services;

[UnitTest]
public class EstadoServiceTests(EfSqliteFixture fixture) : IClassFixture<EfSqliteFixture>
{
    private readonly EfSqliteFixture _fixture = fixture;

    [Fact]
    public async Task Devera_RetornarResultadoSucessoComEstados_AoObterTodos()
    {
        // Arrange
        var service = CriarServico();

        // Act
        var actual = await service.ObterTodosAsync();

        // Assert
        actual.Should().NotBeNull();
        actual.IsSuccess.Should().BeTrue();
        actual.Value.Should().NotBeNullOrEmpty()
            .And.OnlyHaveUniqueItems()
            .And.HaveCount(Totais.Estados)
            .And.Subject.ForEach(e =>
            {
                e.Uf.Should().NotBeNullOrWhiteSpace().And.HaveLength(2);
                e.Regiao.Should().NotBeNullOrWhiteSpace();
                e.Nome.Should().NotBeNullOrWhiteSpace();
            });
    }

    private EstadoService CriarServico()
    {
        var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<DomainToResponseMapper>()));
        var repositorio = new EstadoRepository(_fixture.Context);
        return new EstadoService(mapper, repositorio);
    }
}
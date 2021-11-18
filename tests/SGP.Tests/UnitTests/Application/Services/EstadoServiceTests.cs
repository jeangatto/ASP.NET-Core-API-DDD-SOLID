using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using SGP.Application.Interfaces;
using SGP.Application.Mapper;
using SGP.Application.Services;
using SGP.Infrastructure.Repositories;
using SGP.Tests.Constants;
using SGP.Tests.Extensions;
using SGP.Tests.Fixtures;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.UnitTests.Application.Services
{
    [UnitTest]
    public class EstadoServiceTests : IClassFixture<EfSqliteFixture>
    {
        private readonly EfSqliteFixture _fixture;

        public EstadoServiceTests(EfSqliteFixture fixture) => _fixture = fixture;

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
                    e.Nome.Should().NotBeNullOrEmpty();
                });
        }

        private IEstadoService CriarServico()
        {
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<DomainToResponseMapper>()));
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var repositorio = new EstadoRepository(_fixture.Context);
            return new EstadoService(mapper, memoryCache, repositorio);
        }
    }
}
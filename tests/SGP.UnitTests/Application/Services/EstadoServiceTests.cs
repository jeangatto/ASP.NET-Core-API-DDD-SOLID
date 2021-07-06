using AutoMapper;
using FluentAssertions;
using FluentResults.Extensions.FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using SGP.Application.Interfaces;
using SGP.Application.Mapper;
using SGP.Application.Services;
using SGP.Domain.Repositories;
using SGP.Infrastructure.Repositories;
using SGP.SharedTests.Constants;
using SGP.SharedTests.Extensions;
using SGP.UnitTests.Fixtures;
using System.Threading.Tasks;
using Xunit;
using Xunit.Categories;

namespace SGP.UnitTests.Application.Services
{
    [UnitTest(TestCategories.Application)]
    public class EstadoServiceTests : IClassFixture<EfSqliteFixture>
    {
        private readonly EfSqliteFixture _fixture;

        public EstadoServiceTests(EfSqliteFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Devera_RetornarResultadoSucessoComEstados_QuandoObterTodos()
        {
            // Arrange
            await _fixture.SeedDataAsync();
            var service = CriarServico();

            // Act
            var actual = await service.ObterTodosAsync();

            // Assert
            actual.Should().BeSuccess();
            actual.Value.Should().NotBeNullOrEmpty()
                .And.OnlyHaveUniqueItems()
                .And.HaveCount(Totais.Estados)
                .And.Subject.ForEach(estado =>
                {
                    estado.Uf.Should().NotBeNullOrWhiteSpace().And.HaveLength(2);
                    estado.Regiao.Should().NotBeNullOrWhiteSpace();
                    estado.Nome.Should().NotBeNullOrEmpty();
                });
        }

        private static IMapper CriarMapper()
            => new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<DomainToResponseMapper>()));

        private static IMemoryCache CriarMemoryCache()
            => new MemoryCache(new MemoryCacheOptions());

        private IEstadoRepository CriarRepositorio()
            => new EstadoRepository(_fixture.Context);

        private IEstadoService CriarServico()
            => new EstadoService(CriarMapper(), CriarMemoryCache(), CriarRepositorio());
    }
}

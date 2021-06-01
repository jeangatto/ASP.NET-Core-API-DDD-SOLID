using AutoMapper;
using FluentAssertions;
using FluentResults.Extensions.FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using SGP.Application.Interfaces;
using SGP.Application.Mapper;
using SGP.Application.Requests.CidadeRequests;
using SGP.Application.Services;
using SGP.Domain.Repositories;
using SGP.Infrastructure.Repositories;
using SGP.Tests.Extensions;
using SGP.Tests.Fixtures;
using System.Threading.Tasks;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.UnitTests.Application.Services
{
    [UnitTest(TestCategories.Application)]
    public class CidadeServiceTests : IClassFixture<EfSqliteFixture>
    {
        private readonly EfSqliteFixture _fixture;

        public CidadeServiceTests(EfSqliteFixture fixture) => _fixture = fixture;

        [Fact]
        public async Task Devera_RetornarResultadoSucessoComCidades_QuandoObterTodosPorUf()
        {
            // Arrange
            await _fixture.PopularBaseDadosAsync();
            var service = CriarServico();
            const string uf = "SP";
            var request = new ObterTodosPorUfRequest(uf);

            // Act
            var actual = await service.ObterTodosPorUfAsync(request);

            // Assert
            actual.Should().BeSuccess()
                .And.Subject.Value.Should().OnlyHaveUniqueItems()
                .And.HaveCount(645)
                .And.Subject.ForEach(cidade =>
                {
                    cidade.Regiao.Should().NotBeNullOrWhiteSpace();
                    cidade.Estado.Should().NotBeNullOrWhiteSpace();
                    cidade.Uf.Should().NotBeNullOrWhiteSpace().And.HaveLength(2).And.Be(uf);
                    cidade.Nome.Should().NotBeNullOrWhiteSpace();
                    cidade.Ibge.Should().BePositive();
                });
        }

        [Fact]
        public async Task Devera_RetornarResultadoSucessoComCidade_QuandoObterPorIbge()
        {
            // Arrange
            await _fixture.PopularBaseDadosAsync();
            var service = CriarServico();
            const int ibge = 3557105;
            var request = new ObterPorIbgeRequest(ibge);

            // Act
            var actual = await service.ObterPorIbgeAsync(request);

            // Assert
            actual.Should().BeSuccess();
            actual.Value.Regiao.Should().NotBeNullOrWhiteSpace();
            actual.Value.Estado.Should().NotBeNullOrWhiteSpace();
            actual.Value.Uf.Should().NotBeNullOrWhiteSpace().And.HaveLength(2);
            actual.Value.Nome.Should().NotBeNullOrWhiteSpace();
            actual.Value.Ibge.Should().BePositive().And.Be(ibge);
        }

        private static IMapper CriarMapper()
            => new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<DomainToResponseMapper>()));

        private static IMemoryCache CriarMemoryCache()
            => new MemoryCache(new MemoryCacheOptions());

        private ICidadeRepository CriarRepositorio()
            => new CidadeRepository(_fixture.Context);

        private ICidadeService CriarServico()
            => new CidadeService(CriarMapper(), CriarMemoryCache(), CriarRepositorio());
    }
}
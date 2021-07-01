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
using SGP.Shared.Errors;
using SGP.SharedTests;
using SGP.SharedTests.Constants;
using SGP.SharedTests.Extensions;
using SGP.SharedTests.Fixtures;
using System.Threading.Tasks;
using Xunit;
using Xunit.Categories;

namespace SGP.UnitTests.Application.Services
{
    [UnitTest(TestCategories.Application)]
    public class CidadeServiceTests : IClassFixture<EfSqliteFixture>
    {
        private readonly EfSqliteFixture _fixture;

        public CidadeServiceTests(EfSqliteFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Devera_RetornarErroValidacao_QuandoObterTodosPorUfInvalido()
        {
            // Arrange
            var service = CriarServico();
            var request = new ObterTodosPorUfRequest(string.Empty);

            // Act
            var actual = await service.ObterTodosPorUfAsync(request);

            // Assert
            actual.Should().BeFailure()
                .And.Subject.HasError<ValidationError>().Should().BeTrue();
        }

        [Fact]
        public async Task Devera_RetornarErroNaoEncontrado_QuandoObterTodosPorUfInexistente()
        {
            // Arrange
            await _fixture.SeedDataAsync();
            var service = CriarServico();
            var request = new ObterTodosPorUfRequest("XX");

            // Act
            var actual = await service.ObterTodosPorUfAsync(request);

            // Assert
            actual.Should().BeFailure()
                .And.Subject.HasError<NotFoundError>().Should().BeTrue();
        }

        [Theory]
        [ClassData(typeof(TestDatas.FiltrarPorUf))]
        public async Task Devera_RetornarResultadoSucessoComCidades_QuandoObterTodosPorUf(string uf, int totalEsperado)
        {
            // Arrange
            await _fixture.SeedDataAsync();
            var service = CriarServico();
            var request = new ObterTodosPorUfRequest(uf);

            // Act
            var actual = await service.ObterTodosPorUfAsync(request);

            // Assert
            actual.Should().BeSuccess();
            actual.Value.Should().NotBeNullOrEmpty()
                .And.OnlyHaveUniqueItems()
                .And.HaveCount(totalEsperado)
                .And.Subject.ForEach(cidade =>
                {
                    cidade.Regiao.Should().NotBeNullOrWhiteSpace();
                    cidade.Estado.Should().NotBeNullOrWhiteSpace();
                    cidade.Uf.Should().NotBeNullOrWhiteSpace().And.HaveLength(2).And.Be(uf);
                    cidade.Nome.Should().NotBeNullOrWhiteSpace();
                    cidade.Ibge.Should().BePositive();
                });
        }

        [Theory]
        [ClassData(typeof(TestDatas.FiltrarPorIbge))]
        public async Task Devera_RetornarResultadoSucessoComCidade_QuandoObterPorIbge(int ibge, string cidadeEsperada,
            string ufEsperada, string regiaoEsperada)
        {
            // Arrange
            await _fixture.SeedDataAsync();
            var service = CriarServico();
            var request = new ObterPorIbgeRequest(ibge);

            // Act
            var actual = await service.ObterPorIbgeAsync(request);

            // Assert
            actual.Should().BeSuccess();
            actual.Value.Regiao.Should().NotBeNullOrWhiteSpace().And.Be(regiaoEsperada);
            actual.Value.Estado.Should().NotBeNullOrWhiteSpace();
            actual.Value.Uf.Should().NotBeNullOrWhiteSpace().And.HaveLength(2).And.Be(ufEsperada);
            actual.Value.Nome.Should().NotBeNullOrWhiteSpace().And.Be(cidadeEsperada);
            actual.Value.Ibge.Should().BePositive().And.Be(ibge);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public async Task Devera_RetornarErroValidacao_QuandoObterPorIbgeInvalido(int ibge)
        {
            // Arrange
            var service = CriarServico();
            var request = new ObterPorIbgeRequest(ibge);

            // Act
            var actual = await service.ObterPorIbgeAsync(request);

            // Assert
            actual.Should().BeFailure()
                .And.Subject.HasError<ValidationError>().Should().BeTrue();
        }

        [Fact]
        public async Task Devera_RetornarErroValidacao_QuandoObterPorIbgeInexistente()
        {
            // Arrange
            await _fixture.SeedDataAsync();
            var service = CriarServico();
            var request = new ObterPorIbgeRequest(int.MaxValue);

            // Act
            var actual = await service.ObterPorIbgeAsync(request);

            // Assert
            actual.Should().BeFailure()
                .And.Subject.HasError<NotFoundError>().Should().BeTrue();
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
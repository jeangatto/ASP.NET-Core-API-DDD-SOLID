using System.Threading.Tasks;
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
using SGP.Tests.Constants;
using SGP.Tests.Extensions;
using SGP.Tests.Fixtures;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.UnitTests.Application.Services
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
        public async Task Devera_RetornarErroValidacao_AoObterTodosPorUfInvalido()
        {
            // Arrange
            var service = CriarServico();
            var request = new ObterTodosPorUfRequest(string.Empty);

            // Act
            var actual = await service.ObterTodosPorUfAsync(request);

            // Assert
            actual.Should().BeFailure().And.Subject.HasError<ValidationError>().Should().BeTrue();
        }

        [Fact]
        public async Task Devera_RetornarErroNaoEncontrado_AoObterTodosPorUfInexistente()
        {
            // Arrange
            await _fixture.SeedDataAsync();
            var service = CriarServico();
            var request = new ObterTodosPorUfRequest("XX");

            // Act
            var actual = await service.ObterTodosPorUfAsync(request);

            // Assert
            actual.Should().BeFailure().And.Subject.HasError<NotFoundError>().Should().BeTrue();
        }

        [Fact]
        public async Task Devera_RetornarResultadoSucessoComCidades_AoObterTodosPorUf()
        {
            // Arrange
            await _fixture.SeedDataAsync();
            const string ufSaoPaulo = "SP";
            var request = new ObterTodosPorUfRequest(ufSaoPaulo);
            var service = CriarServico();

            // Act
            var actual = await service.ObterTodosPorUfAsync(request);

            // Assert
            actual.Should().BeSuccess();
            actual.Value.Should().NotBeNullOrEmpty()
                .And.OnlyHaveUniqueItems()
                .And.HaveCount(645)
                .And.Subject.ForEach(c =>
                {
                    c.Regiao.Should().NotBeNullOrWhiteSpace();
                    c.Estado.Should().NotBeNullOrWhiteSpace();
                    c.Uf.Should().NotBeNullOrWhiteSpace().And.HaveLength(2);
                    c.Nome.Should().NotBeNullOrWhiteSpace();
                    c.Ibge.Should().BePositive();
                });
        }

        [Fact]
        public async Task Devera_RetornarResultadoSucessoComCidade_AoObterPorIbge()
        {
            // Arrange
            await _fixture.SeedDataAsync();
            const int ibgeVotuporanga = 3557105;
            var request = new ObterPorIbgeRequest(ibgeVotuporanga);
            var service = CriarServico();

            // Act
            var actual = await service.ObterPorIbgeAsync(request);

            // Assert
            actual.Should().BeSuccess();
            actual.Value.Regiao.Should().NotBeNullOrWhiteSpace();
            actual.Value.Estado.Should().NotBeNullOrWhiteSpace();
            actual.Value.Uf.Should().NotBeNullOrWhiteSpace().And.HaveLength(2);
            actual.Value.Nome.Should().NotBeNullOrWhiteSpace();
            actual.Value.Ibge.Should().BePositive();
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public async Task Devera_RetornarErroValidacao_AoObterPorIbgeInvalido(int ibge)
        {
            // Arrange
            var service = CriarServico();
            var request = new ObterPorIbgeRequest(ibge);

            // Act
            var actual = await service.ObterPorIbgeAsync(request);

            // Assert
            actual.Should().BeFailure().And.Subject.HasError<ValidationError>().Should().BeTrue();
        }

        [Fact]
        public async Task Devera_RetornarErroValidacao_AoObterPorIbgeInexistente()
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
            => new Mapper(new MapperConfiguration(cfg
                => cfg.AddProfile<DomainToResponseMapper>()));

        private static IMemoryCache CriarMemoryCache()
            => new MemoryCache(new MemoryCacheOptions());

        private ICidadeRepository CriarRepositorio()
            => new CidadeRepository(_fixture.Context);

        private ICidadeService CriarServico()
            => new CidadeService(CriarMapper(), CriarMemoryCache(), CriarRepositorio());
    }
}
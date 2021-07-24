using FluentAssertions;
using Moq;
using SGP.Domain.Entities.Rules;
using SGP.Domain.Repositories;
using SGP.Domain.ValueObjects;
using SGP.Shared.Interfaces;
using System.Threading.Tasks;
using Xunit;

namespace SGP.Tests.UnitTests.Domain.Rules
{
    public class UsuarioVerificarDisponibilidadeEmailRuleTests
    {
        [Fact]
        public async Task Devera_RetornarVerdadeiro_QuandoEmailJaExistir()
        {
            // Arrange
            var email = Email.Create("teste.sgp@dominio.com.br");
            var repositoryMock = new Mock<IUsuarioRepository>();
            repositoryMock.Setup(s => s.VerificaSeEmailExisteAsync(It.IsAny<Email>())).ReturnsAsync(true);
            var rule = CreateBusinessRule(email, repositoryMock.Object);

            // Act
            var act = await rule.IsBrokenAsync();

            // Assert
            act.Should().BeTrue();
            repositoryMock.Verify(s
                => s.VerificaSeEmailExisteAsync(It.Is<Email>(x => x == email)), Times.Once);
        }

        [Fact]
        public async Task Devera_RetornarFalso_QuandoEmailNaoExistir()
        {
            // Arrange
            var email = Email.Create("teste.sgp@dominio.com.br");
            var repositoryMock = new Mock<IUsuarioRepository>();
            repositoryMock.Setup(s => s.VerificaSeEmailExisteAsync(It.IsAny<Email>())).ReturnsAsync(false);
            var rule = CreateBusinessRule(email, repositoryMock.Object);

            // Act
            var act = await rule.IsBrokenAsync();

            // Assert
            act.Should().BeFalse();
            repositoryMock.Verify(s
                => s.VerificaSeEmailExisteAsync(It.Is<Email>(x => x == email)), Times.Once);
        }

        private static IBusinessRuleAsync CreateBusinessRule(Email email, IUsuarioRepository repository)
            => new UsuarioVerificarDisponibilidadeEmailRule(email, repository);
    }
}

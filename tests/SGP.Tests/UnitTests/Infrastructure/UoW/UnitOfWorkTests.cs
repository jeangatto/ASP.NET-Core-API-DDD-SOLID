using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SGP.Domain.Entities;
using SGP.Infrastructure.Context;
using SGP.Infrastructure.UoW;
using SGP.Shared.Interfaces;
using SGP.Tests.Fixtures;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SGP.Tests.UnitTests.Infrastructure.UoW
{
    public class UnitOfWorkTests : UnitTestBase, IClassFixture<EfSqliteFixture>
    {
        private readonly EfSqliteFixture _fixture;

        public UnitOfWorkTests(EfSqliteFixture fixture) => _fixture = fixture;

        [Fact]
        public async Task Should_ReturnRowsAffected_WhenSaveChanges()
        {
            // Arrange
            var uow = CreateUoW();
            var context = GetContext();
            context.Add(new Regiao("Norte"));

            // Act
            var actual = await uow.SaveChangesAsync();

            // Assert
            actual.Should().BePositive();
        }

        [Fact]
        public async Task Should_ThrowsException_WhenSaveChanges()
        {
            // Arrange
            var uow = CreateUoW();
            var context = GetContext();
            context.AddRange(new Regiao("Sul"), new Regiao("Sul")); // Duplicate Index

            // Act
            Func<Task> actual = async () => await uow.SaveChangesAsync();

            // Assert
            await actual.Should().ThrowAsync<Exception>();
        }

        private IUnitOfWork CreateUoW()
            => new UnitOfWork(_fixture.Context, Mock.Of<ILogger<UnitOfWork>>());

        private SgpContext GetContext()
        {
            _fixture.Context.ChangeTracker.Clear();
            return _fixture.Context;
        }
    }
}
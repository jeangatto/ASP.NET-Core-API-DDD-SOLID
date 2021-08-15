using System;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SGP.Infrastructure.Context;
using SGP.Tests.Mocks;
using Xunit;

namespace SGP.Tests.Fixtures
{
    public class EfSqliteFixture : IAsyncLifetime, IDisposable
    {
        private const string ConnectionString = "Data Source=:memory:";
        private readonly SqliteConnection _connection;

        public EfSqliteFixture()
        {
            _connection = new SqliteConnection(ConnectionString);
            _connection.Open();

            var dbContextOptionsBuilder = new DbContextOptionsBuilder<SgpContext>()
                .UseSqlite(_connection)
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging();

            Context = new SgpContext(dbContextOptionsBuilder.Options);
            Context.Database.EnsureDeleted();
            Context.Database.EnsureCreated();
        }

        public async Task InitializeAsync() => await Context.EnsureSeedDataAsync(LoggerFactoryMock.Create());

        public Task DisposeAsync() => Task.CompletedTask;

        public SgpContext Context { get; }

        #region Dispose

        // To detect redundant calls.
        private bool _disposed;

        // Public implementation of Dispose pattern callable by consumers.
        ~EfSqliteFixture()
        {
            Dispose(false);
        }

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            // Dispose managed state (managed objects).
            if (disposing)
            {
                _connection?.Dispose();
                Context?.Dispose();
            }

            _disposed = true;
        }

        #endregion
    }
}
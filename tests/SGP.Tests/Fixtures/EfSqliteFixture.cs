using System;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SGP.Infrastructure.Context;
using SGP.Tests.Mocks;

namespace SGP.Tests.Fixtures
{
    public class EfSqliteFixture : IDisposable
    {
        private readonly SqliteConnection _connection;
        private const string ConnectionString = "DataSource=:memory:";

        public EfSqliteFixture()
        {
            _connection = new SqliteConnection(ConnectionString);
            _connection.Open();

            var options = new DbContextOptionsBuilder<SgpContext>()
                .UseSqlite(_connection)
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
                .Options;

            Context = new SgpContext(options);
            Context.Database.EnsureDeleted();
            Context.Database.EnsureCreated();
        }

        public SgpContext Context { get; }

        public async Task SeedDataAsync()
        {
            await Context.EnsureSeedDataAsync(LoggerFactoryMock.Create());
        }

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
            {
                return;
            }

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
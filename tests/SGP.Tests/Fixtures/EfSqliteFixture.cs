using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SGP.Infrastructure.Context;
using SGP.Tests.Mocks;
using System;
using System.Threading.Tasks;

namespace SGP.Tests.Fixtures
{
    public class EfSqliteFixture : IDisposable
    {
        private const string _connectionString = "DataSource=:memory:";
        private readonly SqliteConnection _connection;

        public EfSqliteFixture()
        {
            _connection = new SqliteConnection(_connectionString);
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

        public void Popular() => Context.EnsureSeedDataAsync(LoggerFactoryMock.Create()).Wait();

        public async Task PopularAsync() => await Context.EnsureSeedDataAsync(LoggerFactoryMock.Create());

        #region Dispose

        // REF: https://docs.microsoft.com/pt-br/dotnet/standard/garbage-collection/implementing-dispose
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
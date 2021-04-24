using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SGP.Infrastructure.Context;
using System;

namespace SGP.Tests.Fixtures
{
    public class EfFixture : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly string _connectionString = "DataSource=:memory:";

        public EfFixture()
        {
            _connection = new SqliteConnection(_connectionString);
            _connection.Open();

            var builder = new DbContextOptionsBuilder<SgpContext>().UseSqlite(_connection);

            Context = new SgpContext(builder.Options);
            Context.Database.EnsureDeleted();
            Context.Database.EnsureCreated();
        }

        public SgpContext Context { get; }

        #region Dispose

        // REF: https://docs.microsoft.com/pt-br/dotnet/standard/garbage-collection/implementing-dispose
        // To detect redundant calls.
        private bool _disposed;

        // Public implementation of Dispose pattern callable by consumers.
        ~EfFixture()
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
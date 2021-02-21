using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SGP.Infrastructure.Context;

namespace SGP.ConsoleApp
{
    public static class Program
    {
        private const string ConnectionString
            = "Server=(localdb)\\mssqllocaldb;Database=SGPContexto;Trusted_Connection=True;MultipleActiveResultSets=true";

        public static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddDbContext<SGPContext>(builder => builder.UseSqlServer(ConnectionString));

            var serviceProvider = services.BuildServiceProvider(new ServiceProviderOptions
            {
                ValidateOnBuild = true,
                ValidateScopes = true
            });

            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<SGPContext>();
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
        }
    }
}

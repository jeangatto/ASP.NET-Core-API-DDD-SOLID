using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using SGP.PublicApi.Extensions;

namespace SGP.PublicApi;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        await host.MigrateDbContextAsync();
        await host.RunAsync();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
        => Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder
                    .UseKestrel(options =>
                    {
                        options.AddServerHeader = false;
                        options.AllowSynchronousIO = true;
                    })
                    .UseStartup<Startup>();
            })
            .UseDefaultServiceProvider((context, options) =>
            {
                options.ValidateScopes = context.HostingEnvironment.IsDevelopment();
                options.ValidateOnBuild = true;
            });
}
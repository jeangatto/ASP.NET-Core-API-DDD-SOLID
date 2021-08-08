using GraphQL;
using GraphQL.Caching;
using GraphQL.Execution;
using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using GraphQL.Types;
using GraphQL.Validation;
using GraphQL.Validation.Complexity;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Scrutor;
using SGP.PublicApi.GraphQL.Constants;
using SGP.PublicApi.GraphQL.Schemas;

namespace SGP.PublicApi.Extensions
{
    public static class ServicesCollectionExtensions
    {
        public static void AddGraphQLWithSchemas(this IServiceCollection services)
        {
            services
                .AddSchemas()
                .AddDocumentExecuter()
                .AddGraphQL((options, provider) =>
                {
                    options.EnableMetrics = true;
                    var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
                    var logger = loggerFactory.CreateLogger("GraphQL");
                    options.UnhandledExceptionDelegate = context =>
                    {
                        var ex = context.OriginalException;
                        var message = $"Ocorreu um erro com GraphQL, mensagem: {ex.Message}";
                        logger.LogError(ex, message);
                    };
                })
                .AddDataLoader()
                .AddNewtonsoftJson(_ => { }, _ => { })
                .AddGraphTypes(ServiceLifetime.Scoped)
                .AddErrorInfoProvider((options, provider) =>
                {
                    var environment = provider.GetRequiredService<IHostEnvironment>();
                    options.ExposeExceptionStackTrace = environment.IsDevelopment();
                });
        }

        public static void UseGraphQL(this IApplicationBuilder app)
        {
            // NOTE: Buscar uma forma de fazer via Reflection para evitar repetição de código.
            app.UseGraphQL<CidadeSchema>(GraphQLApiEndpoints.Cidades);
            app.UseGraphQLPlayground(new PlaygroundOptions { GraphQLEndPoint = GraphQLApiEndpoints.Cidades },
                GraphQLPlaygroundEndpoints.Cidades);

            app.UseGraphQL<EstadoSchema>(GraphQLApiEndpoints.Estados);
            app.UseGraphQLPlayground(new PlaygroundOptions { GraphQLEndPoint = GraphQLApiEndpoints.Estados },
                GraphQLPlaygroundEndpoints.Estados);
        }

        private static IServiceCollection AddDocumentExecuter(this IServiceCollection services)
        {
            // Document Caching
            // REF: https://graphql-dotnet.github.io/docs/guides/document-caching/
            var documentCache = new MemoryDocumentCache(new MemoryDocumentCacheOptions
            {
                // maximum total cached query length of 1,000,000 bytes (assume 10x memory usage
                // for 10MB maximum memory use by the cache)
                SizeLimit = 1000000,
                // no expiration of cached queries (cached queries are only ejected when the cache is full)
                SlidingExpiration = null
            });

            services.AddSingleton<IDocumentExecuter>(_ => new DocumentExecuter(
                new GraphQLDocumentBuilder(),
                new DocumentValidator(),
                new ComplexityAnalyzer(),
                documentCache));

            return services;
        }

        private static IServiceCollection AddSchemas(this IServiceCollection services)
        {
            services.Scan(scan => scan
                .FromCallingAssembly()
                .AddClasses(@class => @class.AssignableTo<Schema>())
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsSelf()
                .WithScopedLifetime());

            return services;
        }
    }
}
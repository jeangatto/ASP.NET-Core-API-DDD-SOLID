using Ardalis.GuardClauses;
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
        public static IServiceCollection AddGraphQLWithSchemas(this IServiceCollection services)
        {
            Guard.Against.Null(services, nameof(services));

            services
                .AddSchemas()
                .AddDocumentExecuter()
                .AddGraphQL((options, serviceProvider) =>
                {
                    options.EnableMetrics = true;
                    var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
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
                .AddErrorInfoProvider((options, serviceProvider) =>
                {
                    var environment = serviceProvider.GetRequiredService<IHostEnvironment>();
                    options.ExposeExceptionStackTrace = environment.IsDevelopment();
                });

            return services;
        }

        public static IApplicationBuilder UseGraphQL(this IApplicationBuilder app)
        {
            Guard.Against.Null(app, nameof(app));

            // NOTE: Buscar uma forma de fazer via Reflection para evitar repetição de código.

            app.UseGraphQL<CidadeSchema>(GraphQLApiEndpoints.Cidades);
            app.UseGraphQLPlayground(new PlaygroundOptions
            {
                GraphQLEndPoint = GraphQLApiEndpoints.Cidades
            }, path: GraphQLPlaygroundEndpoints.Cidades);

            app.UseGraphQL<EstadoSchema>(GraphQLApiEndpoints.Estados);
            app.UseGraphQLPlayground(new PlaygroundOptions
            {
                GraphQLEndPoint = GraphQLApiEndpoints.Estados
            }, path: GraphQLPlaygroundEndpoints.Estados);

            return app;
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
                SlidingExpiration = null,
            });

            services.AddSingleton<IDocumentExecuter>(_ =>
            {
                return new DocumentExecuter(
                    new GraphQLDocumentBuilder(),
                    new DocumentValidator(),
                    new ComplexityAnalyzer(),
                    documentCache);
            });

            return services;
        }

        private static IServiceCollection AddSchemas(this IServiceCollection services)
        {
            services.Scan(scan => scan
                .FromCallingAssembly()
                .AddClasses(implementations => implementations.AssignableTo<Schema>())
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsSelf()
                .WithScopedLifetime());

            return services;
        }
    }
}

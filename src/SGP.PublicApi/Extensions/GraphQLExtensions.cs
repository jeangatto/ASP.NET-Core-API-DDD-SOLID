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
    internal static class GraphQLExtensions
    {
        private const string LoggerCategoryName = "GraphQL";

        internal static IServiceCollection AddGraphQLWithSchemas(this IServiceCollection services)
        {
#pragma warning disable CS0612
            services
                .AddSchemas()
                .AddDocumentExecuter()
                .AddGraphQL((options, provider) =>
                {
                    options.EnableMetrics = true;
                    options.UnhandledExceptionDelegate = context =>
                    {
                        var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
                        var logger = loggerFactory.CreateLogger(LoggerCategoryName);
                        logger.LogError(context.OriginalException, "Ocorreu um erro com o serviço do GraphQL");
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
#pragma warning restore CS0612

            return services;
        }

        internal static IApplicationBuilder UseGraphQL(this IApplicationBuilder app)
        {
            app.UseGraphQL<CidadeSchema>(EndPoints.Api.Cidades)
               .UseGraphQLPlayground(new PlaygroundOptions
               {
                   GraphQLEndPoint = EndPoints.Api.Cidades
               }, EndPoints.Ui.Cidades);

            app.UseGraphQL<EstadoSchema>(EndPoints.Api.Estados)
               .UseGraphQLPlayground(new PlaygroundOptions
               {
                   GraphQLEndPoint = EndPoints.Api.Estados
               }, EndPoints.Ui.Estados);

            return app;
        }

        private static IServiceCollection AddDocumentExecuter(this IServiceCollection services)
        {
            // Document Caching
            // REF: https://graphql-dotnet.github.io/docs/guides/document-caching/
            var documentCache = new MemoryDocumentCache(new MemoryDocumentCacheOptions
            {
                // Maximum total cached query length of 1,000,000 bytes (assume 10x memory usage
                // for 10MB maximum memory use by the cache)
                SizeLimit = 1000000,
                // No expiration of cached queries (cached queries are only ejected when the cache is full)
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
            => services.Scan(scan => scan
                .FromCallingAssembly()
                .AddClasses(classes => classes.AssignableTo<Schema>())
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsSelf()
                .WithScopedLifetime());
    }
}
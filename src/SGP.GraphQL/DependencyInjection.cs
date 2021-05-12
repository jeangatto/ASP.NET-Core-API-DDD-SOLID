using Ardalis.GuardClauses;
using GraphQL;
using GraphQL.Caching;
using GraphQL.Execution;
using GraphQL.NewtonsoftJson;
using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using GraphQL.Validation;
using GraphQL.Validation.Complexity;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SGP.GraphQL.Schemas;

namespace SGP.GraphQL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddGraphQLWithSchemas(this IServiceCollection services)
        {
            Guard.Against.Null(services, nameof(services));

            // Document Caching
            // REF: https://graphql-dotnet.github.io/docs/guides/document-caching/
            services.AddSingleton<IDocumentCache>(_ =>
            {
                return new MemoryDocumentCache(new MemoryDocumentCacheOptions
                {
                    // maximum total cached query length of 1,000,000 bytes (assume 10x memory usage
                    // for 10MB maximum memory use by the cache)
                    SizeLimit = 1000000,
                    // no expiration of cached queries (cached queries are only ejected when the cache is full)
                    SlidingExpiration = null,
                });
            });
            services.AddSingleton<IDocumentExecuter>(serviceProvider =>
            {
                return new DocumentExecuter(
                    new GraphQLDocumentBuilder(),
                    new DocumentValidator(),
                    new ComplexityAnalyzer(),
                    serviceProvider.GetRequiredService<IDocumentCache>());
            });
            services.AddSingleton<IDocumentWriter, DocumentWriter>();

            // Schemas GraphQL
            services.AddScoped<CitySchema>();
            services.AddScoped<UserSchema>();

            services.AddGraphQL((options, serviceProvider) =>
            {
                options.EnableMetrics = true;
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger("GraphQL");
                options.UnhandledExceptionDelegate = context => logger.LogError(context.OriginalException, $"Ocorreu um erro com GraphQL, mensagem: {context.OriginalException.Message}");
            })
            .AddDataLoader()
            .AddNewtonsoftJson()
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

            app.UseGraphQL<CitySchema>("/api/cities");
            app.UseGraphQLPlayground(new PlaygroundOptions
            {
                GraphQLEndPoint = "/api/cities"
            }, path: "/ui/cities/graphql");

            app.UseGraphQL<UserSchema>("/api/users");
            app.UseGraphQLPlayground(new PlaygroundOptions
            {
                GraphQLEndPoint = "/api/users"
            }, path: "/ui/users/graphql");

            return app;
        }
    }
}

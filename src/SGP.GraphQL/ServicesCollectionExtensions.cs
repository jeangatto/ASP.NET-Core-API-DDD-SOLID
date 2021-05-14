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
using SGP.GraphQL.Schemas;

namespace SGP.GraphQL
{
    public static class DependencyInjection
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

            app.UseGraphQL<CitySchema>("/api/cities")
                .UseGraphQLPlayground(new PlaygroundOptions
                {
                    GraphQLEndPoint = "/api/cities"
                }, path: "/ui/cities/graphql");

            app.UseGraphQL<UserSchema>("/api/users")
                .UseGraphQLPlayground(new PlaygroundOptions
                {
                    GraphQLEndPoint = "/api/users"
                }, path: "/ui/users/graphql");

            return app;
        }

        private static IServiceCollection AddDocumentExecuter(this IServiceCollection services)
        {
            // Document Caching
            // REF: https://graphql-dotnet.github.io/docs/guides/document-caching/
            var documentCache = new MemoryDocumentCache(new MemoryDocumentCacheOptions
            {
                SizeLimit = 1000000,
                SlidingExpiration = null
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
                .AddClasses(classes => classes.AssignableTo<Schema>())
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsSelf()
                .WithScopedLifetime());

            return services;
        }
    }
}

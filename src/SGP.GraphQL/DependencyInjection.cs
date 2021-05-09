using Ardalis.GuardClauses;
using GraphQL;
using GraphQL.NewtonsoftJson;
using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SGP.Infrastructure.GraphQL;

namespace SGP.GraphQL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddConfiguredGraphQL(this IServiceCollection services)
        {
            Guard.Against.Null(services, nameof(services));

            services.AddScoped<IDocumentExecuter, DocumentExecuter>();
            services.AddScoped<IDocumentWriter, DocumentWriter>();
            services.AddScoped<ISchema, SgpSchema>();

            services.AddGraphQL(options => options.EnableMetrics = true)
                .AddDataLoader()
                .AddGraphTypes(ServiceLifetime.Scoped)
                .AddErrorInfoProvider(options => options.ExposeExceptionStackTrace = true)
                .AddNewtonsoftJson();

            return services;
        }

        public static IApplicationBuilder UseGraphQL(this IApplicationBuilder app)
        {
            Guard.Against.Null(app, nameof(app));

            app.UseGraphQL<ISchema>();
            app.UseGraphQLPlayground(new PlaygroundOptions
            {
                EditorTheme = EditorTheme.Dark,
            }, path: "/ui/graphql");
            return app;
        }
    }
}

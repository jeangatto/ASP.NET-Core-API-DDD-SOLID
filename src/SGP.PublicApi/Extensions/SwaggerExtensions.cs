using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace SGP.PublicApi.Extensions
{
    public static class SwaggerExtensions
    {
        private const string Title = "SGP";
        private const string Version = "v1";

        public static IServiceCollection AddOpenApi(this IServiceCollection services)
        {
            Guard.Against.Null(services, nameof(services));

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(Version, new OpenApiInfo
                {
                    Version = Version,
                    Title = Title,
                    Description = "Sistema Gerenciador de Pedidos Online",
                    Contact = new OpenApiContact
                    {
                        Name = "Jean Gatto",
                        Email = "jean_gatto@hotmail.com",
                        Url = new Uri("https://www.linkedin.com/in/jeangatto/")
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });

            return services;
        }

        public static IApplicationBuilder UseOpenApi(this IApplicationBuilder app)
        {
            Guard.Against.Null(app, nameof(app));

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.DisplayRequestDuration();
                options.SwaggerEndpoint("/swagger/v1/swagger.json", $"{Title} {Version}");
            });

            return app;
        }
    }
}
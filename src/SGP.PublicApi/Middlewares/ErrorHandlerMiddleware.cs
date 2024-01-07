using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SGP.PublicApi.Models;
using SGP.Shared.Extensions;

namespace SGP.PublicApi.Middlewares;

public class ErrorHandlerMiddleware(
    RequestDelegate next,
    ILogger<ErrorHandlerMiddleware> logger,
    IHostEnvironment environment)
{
    private const string DefaultErrorMessage = "Ocorreu um erro interno ao processar a sua solicitação.";

    private readonly RequestDelegate _next = next;
    private readonly ILogger<ErrorHandlerMiddleware> _logger = logger;
    private readonly IHostEnvironment _environment = environment;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Foi gerada uma exceção não esperada: {Message}", ex.Message);

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            if (_environment.IsDevelopment())
            {
                context.Response.ContentType = MediaTypeNames.Text.Plain;
                await context.Response.WriteAsync(ex.ToString());
            }
            else
            {
                context.Response.ContentType = MediaTypeNames.Application.Json;
                await context.Response.WriteAsync(ApiResponse.InternalServerError(DefaultErrorMessage).ToJson());
            }
        }
    }
}
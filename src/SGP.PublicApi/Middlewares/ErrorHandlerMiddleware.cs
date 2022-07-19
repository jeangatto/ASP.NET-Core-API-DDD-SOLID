using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SGP.PublicApi.Models;
using SGP.Shared.Extensions;

namespace SGP.PublicApi.Middlewares;

public class ErrorHandlerMiddleware
{
    private const string ErrorMessage = "Ocorreu um erro interno ao processar a sua solicitação.";

    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;
    private readonly IHostEnvironment _environment;

    public ErrorHandlerMiddleware(
        RequestDelegate next,
        ILogger<ErrorHandlerMiddleware> logger,
        IHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Foi gerada uma exceção não esperada: {Message}", ex.Message);

            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            // NOTE: Quando for ambiente de desenvolvimento, será exibida a stack trace completa da exception.
            var response = _environment.IsDevelopment()
                ? new ApiResponse(false, StatusCodes.Status500InternalServerError, ex.ToJson()).ToJson()
                : new ApiResponse(false, StatusCodes.Status500InternalServerError, ErrorMessage).ToJson();

            await context.Response.WriteAsync(response);
        }
    }
}
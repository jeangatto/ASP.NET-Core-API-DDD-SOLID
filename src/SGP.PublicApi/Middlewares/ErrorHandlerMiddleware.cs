using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SGP.PublicApi.Models;
using SGP.Shared.Extensions;

namespace SGP.PublicApi.Middlewares;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;
    private const int ErrorStatusCode = StatusCodes.Status500InternalServerError;
    private const string ErrorMessage = "Ocorreu um erro interno ao processar a sua solicitação.";

    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
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
            context.Response.StatusCode = ErrorStatusCode;
            await context.Response.WriteAsync(new ApiResponse(false, ErrorStatusCode, ErrorMessage).ToJson());
        }
    }
}
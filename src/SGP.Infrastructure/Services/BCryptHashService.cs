using System;
using Microsoft.Extensions.Logging;
using SGP.Shared.Interfaces;
using Throw;

namespace SGP.Infrastructure.Services;

public class BCryptHashService : IHashService
{
    private readonly ILogger<BCryptHashService> _logger;

    public BCryptHashService(ILogger<BCryptHashService> logger) => _logger = logger;

    public bool Compare(string text, string hash)
    {
        text.ThrowIfNull().IfEmpty().IfWhiteSpace();
        hash.ThrowIfNull().IfEmpty().IfWhiteSpace();

        try
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(text, hash);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao verificar o HASH com BCrypt");
            throw;
        }
    }

    public string Hash(string text)
    {
        text.ThrowIfNull().IfEmpty().IfWhiteSpace();

        try
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(text);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao gerar o HASH com BCrypt");
            throw;
        }
    }
}
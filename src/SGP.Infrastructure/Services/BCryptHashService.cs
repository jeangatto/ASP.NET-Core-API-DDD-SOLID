using System;
using Ardalis.GuardClauses;
using Microsoft.Extensions.Logging;
using SGP.Shared.Abstractions;

namespace SGP.Infrastructure.Services;

public class BCryptHashService : IHashService
{
    private readonly ILogger<BCryptHashService> _logger;

    public BCryptHashService(ILogger<BCryptHashService> logger) => _logger = logger;

    public bool Compare(string text, string hash)
    {
        Guard.Against.NullOrWhiteSpace(text);
        Guard.Against.NullOrWhiteSpace(hash);

        try
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(text, hash);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao verificar o HASH com BCrypt: {message}", ex.Message);
            throw;
        }
    }

    public string Hash(string text)
    {
        Guard.Against.NullOrWhiteSpace(text);

        try
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(text);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao gerar o HASH com BCrypt: {message}", ex.Message);
            throw;
        }
    }
}
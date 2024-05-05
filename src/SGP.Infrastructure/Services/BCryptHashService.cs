using System;
using Ardalis.GuardClauses;
using Microsoft.Extensions.Logging;
using SGP.Shared.Abstractions;

namespace SGP.Infrastructure.Services;

public class BCryptHashService(ILogger<BCryptHashService> logger) : IHashService
{
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
            logger.LogError(ex, "Ocorreu um erro ao verificar o HASH com BCrypt: {Message}", ex.Message);
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
            logger.LogError(ex, "Ocorreu um erro ao gerar o HASH com BCrypt: {Message}", ex.Message);
            throw;
        }
    }
}
using Microsoft.Extensions.Logging;
using SGP.Shared.Interfaces;
using System;

namespace SGP.Infrastructure.Services
{
    public class HashService : IHashService
    {
        private readonly ILogger<HashService> _logger;

        public HashService(ILogger<HashService> logger)
        {
            _logger = logger;
        }

        public bool Compare(string text, string hash)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(text, hash);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, $"Ocorreu uma exceção ao verificar o HASH com BCrypt, text: '{text}', hash: '{hash}', exceção: {ex.Message}");
                throw;
            }
        }

        public string Hash(string text)
        {
            try
            {
                return BCrypt.Net.BCrypt.HashPassword(text);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, $"Ocorreu uma exceção ao gerar o HASH com BCrypt, text: '{text}', exceção: {ex.Message}");
                throw;
            }
        }
    }
}

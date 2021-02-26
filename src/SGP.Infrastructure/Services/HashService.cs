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
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentNullException(nameof(text));

            if (string.IsNullOrWhiteSpace(hash))
                throw new ArgumentNullException(nameof(hash));

            try
            {
                return BCrypt.Net.BCrypt.EnhancedVerify(text, hash);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, $"Ocorreu uma exceção ao verificar o HASH com BCrypt, text: '{text}', hash: '{hash}', exceção: {ex.Message}");
                throw;
            }
        }

        public string Hash(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentNullException(nameof(text));

            try
            {
                return BCrypt.Net.BCrypt.EnhancedHashPassword(text);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, $"Ocorreu uma exceção ao gerar o HASH com BCrypt, text: '{text}', exceção: {ex.Message}");
                throw;
            }
        }
    }
}

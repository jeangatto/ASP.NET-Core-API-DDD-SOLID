using SGP.Shared.Entities;
using System.Collections.Generic;
using System.Security.Claims;

namespace SGP.Shared.Interfaces
{
    /// <summary>
    /// Serviço de geração de Json Web Token.
    /// </summary>
    public interface IJWTokenService
    {
        /// <summary>
        /// Gera a autenticação como Json Web Tokens.
        /// </summary>
        /// <param name="claims">Declarações do emissor.</param>
        JWToken GenerateToken(IEnumerable<Claim> claims);
    }
}
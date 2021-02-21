using SGP.Shared.Entities;
using System.Collections.Generic;

namespace SGP.Domain.ValueObjects
{
    /// <summary>
    /// Cadastro de Pessoa Física (CPF).
    /// </summary>
    public class CadastroPessoaFisica : ValueObject
    {
        public CadastroPessoaFisica(string numero)
        {
            Numero = numero;
        }

        /// <summary>
        /// Número do Documento.
        /// </summary>
        public string Numero { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Numero;
        }
    }
}

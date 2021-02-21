using SGP.Shared.Entities;
using System.Collections.Generic;

namespace SGP.Domain.ValueObjects
{
    /// <summary>
    /// Cadastro Nacional da Pessoa Jurídica (CNPJ).
    /// </summary>
    public class CadastroNacionalPessoaJuridica : ValueObject
    {
        public CadastroNacionalPessoaJuridica(string numero)
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
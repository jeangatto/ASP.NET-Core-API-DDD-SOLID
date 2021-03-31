using System;

namespace SGP.Shared.Interfaces
{
    /// <summary>
    /// Serviço de Data e Hora.
    /// </summary>
    public interface IDateTime
    {
        /// <summary>
        /// Obtém a data e hora atual.
        /// </summary>
        DateTime Now { get; }
    }
}
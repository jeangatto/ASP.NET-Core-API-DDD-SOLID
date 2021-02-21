using SGP.Shared.Entities;
using System;
using System.Collections.Generic;

namespace SGP.Domain.ValueObjects
{
    public class DataCadastro : ValueObject
    {
        public DataCadastro(DateTime data)
        {
            Data = data;
        }

        public DateTime Data { get; }

        public static DataCadastro Agora() => new DataCadastro(DateTime.Now);

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Data;
        }
    }
}

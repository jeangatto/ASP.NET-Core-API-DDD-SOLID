using SGP.Shared.Entities;
using System;
using System.Collections.Generic;

namespace SGP.Domain.ValueObjects
{
    public class DataNascimento : ValueObject
    {
        public DataNascimento(DateTime data)
        {
            Data = data;
        }

        public DateTime Data { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Data;
        }
    }
}

using System.Collections.Generic;
using SGP.Shared.Abstractions;

namespace SGP.Domain.Entities;

public class Regiao : BaseEntity
{
    public Regiao(string nome) =>
        Nome = nome;

    public Regiao() // ORM
    {
    }

    public string Nome { get; private init; }

    public IReadOnlyList<Estado> Estados { get; private init; }
}
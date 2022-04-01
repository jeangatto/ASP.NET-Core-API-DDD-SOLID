using System.Collections.Generic;
using SGP.Shared.Entities;

namespace SGP.Domain.Entities;

public class Regiao : BaseEntity
{
    public Regiao(string nome)
    {
        Nome = nome;
    }

    public Regiao() // ORM
    {
    }

    public string Nome { get; private set; }

    public IReadOnlyList<Estado> Estados { get; private set; }
}
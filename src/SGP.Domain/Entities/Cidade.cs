using System;
using SGP.Shared.Abstractions;

namespace SGP.Domain.Entities;

public class Cidade : BaseEntity
{
    public Cidade(Guid estadoId, string nome, int ibge)
    {
        EstadoId = estadoId;
        Nome = nome;
        Ibge = ibge;
    }

    private Cidade() // ORM
    {
    }

    public Guid EstadoId { get; private init; }
    public string Nome { get; private init; }
    public int Ibge { get; private init; }

    public Estado Estado { get; private init; }
}
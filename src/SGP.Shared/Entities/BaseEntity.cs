using System;
using SGP.Shared.Abstractions;

namespace SGP.Shared.Entities;

/// <summary>
/// Entidade com a chave (PK) tipada em <see cref="Guid"/>.
/// </summary>
public abstract class BaseEntity : IEntityKey<Guid>
{
    public Guid Id { get; private init; } = Guid.NewGuid();
}
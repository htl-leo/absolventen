using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

/// <summary>
/// Abstrakte Basisklasse für Entitäten mit Id und Concurrency-Token.
/// </summary>
public abstract class BaseEntity : IBaseEntity
{
    /// <summary>
    /// Primärschlüssel (Identity). Protected set, damit nur EF/abgeleitete Klassen setzen können.
    /// </summary>
    public int Id { get; private init; }

    /// <summary>
    /// Concurrency-Token (RowVersion). EF setzt diesen Wert bei jeder Änderung.
    /// Dient zur Erkennung konkurrierender Updates.
    /// </summary>
    [Timestamp]
    public byte[] RowVersion { get; set; } = [];
}

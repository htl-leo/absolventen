using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

/// <summary>
/// Basisvertrag für alle Entitäten in der Domänenschicht.
/// </summary>
public interface IBaseEntity
{
    /// <summary>
    /// Primärschlüssel. Wird von EF Core gesetzt.
    /// </summary>
    int Id { get; }

    /// <summary>
    /// Concurrency-Token zur Absicherung gegen gleichzeitige Updates (SQL Server rowversion/timestamp).
    /// EF Core nutzt dieses Feld, um Optimistic Concurrency zu unterstützen.
    /// </summary>
    [Timestamp]
    byte[] RowVersion { get; set; }
}

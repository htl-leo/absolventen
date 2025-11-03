using Application.Contracts.Repositories;

namespace Application.Contracts;

/// <summary>
/// Aggregiert Repositories und speichert Änderungen. Sicherer Umgang mit Transaktionen.
/// </summary>
public interface IUnitOfWork
{
    ISensorRepository Sensors { get; }
    IMeasurementRepository Measurements { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}

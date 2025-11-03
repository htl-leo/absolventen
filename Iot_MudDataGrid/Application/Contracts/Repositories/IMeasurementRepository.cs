using Domain.Entities;

namespace Application.Contracts.Repositories;

/// <summary>
/// Abfragen für Messungen inkl. Paging und Zählen.
/// </summary>
public interface IMeasurementRepository : IGenericRepository<Measurement>
{
    Task<IReadOnlyCollection<Measurement>> GetBySensorAsync(string location, string name, CancellationToken ct = default);
    Task<int> CountBySensorIdAsync(int sensorId, CancellationToken ct = default);
    Task<IReadOnlyCollection<Measurement>> GetBySensorIdPagedAsync(int sensorId, int skip, int take, CancellationToken ct = default);
    // Stream alle Messungen (sequenzielles Lesen, ohne alles vorab zu puffern)
    IAsyncEnumerable<Measurement> StreamAllAsync(CancellationToken ct = default);
    // Alle Messungen inkl. Sensor (Include) für DTO-Projektion
    Task<IReadOnlyCollection<Measurement>> GetAllWithSensorAsync(CancellationToken ct = default);
    // Stream mit Sensor-Join
    IAsyncEnumerable<Measurement> StreamAllWithSensorAsync(CancellationToken ct = default);
}

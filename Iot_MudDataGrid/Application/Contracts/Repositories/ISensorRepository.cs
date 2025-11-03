using Application.Features.Dtos;
using Domain.Entities;

namespace Application.Contracts.Repositories;

/// <summary>
/// Sensor-spezifische Abfragen zusätzlich zu den generischen CRUDs.
/// </summary>
public interface ISensorRepository : IGenericRepository<Sensor>
{
    Task<IReadOnlyCollection<GetSensorWithNumberOfMeasurementsDto>> GetAllWithNumberOfMeasurementsAsync(CancellationToken ct);
    Task<Sensor?> GetByLocationAndNameAsync(string location, string name, CancellationToken ct = default);
}

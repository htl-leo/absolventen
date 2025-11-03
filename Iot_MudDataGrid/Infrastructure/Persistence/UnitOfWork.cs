using Application.Contracts;
using Application.Contracts.Repositories;
using Infrastructure.Persistence.Repositories;

namespace Infrastructure.Persistence;

/// <summary>
/// Unit of Work aggregiert Repositories und speichert Änderungen transaktional.
/// </summary>
public class UnitOfWork(AppDbContext dbContext, 
        ISensorRepository sensors, IMeasurementRepository measurements) : IUnitOfWork, IDisposable
{
    private readonly AppDbContext _dbContext = dbContext;
    private bool _disposed;

    /// <summary>
    /// Zugriff auf Sensor-Repository.
    /// </summary>
    public ISensorRepository Sensors { get; } = sensors;

    /// <summary>
    /// Zugriff auf Measurement-Repository.
    /// </summary>
    public IMeasurementRepository Measurements { get; } = measurements;

    /// <summary>
    /// Persistiert alle Änderungen in die DB. Gibt die Anzahl der betroffenen Zeilen zurück.
    /// </summary>
    public Task<int> SaveChangesAsync(CancellationToken ct = default) => _dbContext.SaveChangesAsync(ct);

    /// <summary>
    /// Gibt verwaltete Ressourcen frei. Der DbContext gehört zum Scope dieser UoW und wird hier entsorgt.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;
        if (disposing)
        {
            _dbContext.Dispose();
        }
        _disposed = true;
    }
}

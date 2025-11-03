using Application.Contracts.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repositories;

/// <summary>
/// Generische Repository-Implementierung für CRUD-Operationen.
/// Nutzt EF Core DbSet<T> für Datenzugriffe.
/// </summary>
public class GenericRepository<T>(AppDbContext dbContext) : IGenericRepository<T> where T : class, IBaseEntity
{
    protected AppDbContext DbContext { get; } = dbContext;
    protected DbSet<T> Set { get; } = dbContext.Set<T>();

    /// <summary>
    /// Holt eine Entität per Primärschlüssel.
    /// </summary>
    public virtual async Task<T?> GetByIdAsync(int id, CancellationToken ct = default)
        => await Set.FindAsync([id], ct);

    /// <summary>
    /// Holt alle Entitäten (optional gefiltert und sortiert). NoTracking für read-only.
    /// </summary>
    public virtual async Task<IReadOnlyCollection<T>> GetAllAsync(
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Expression<Func<T, bool>>? filter = null,
        CancellationToken ct = default)
    {
        IQueryable<T> query = Set.AsNoTracking();
        if (filter is not null)
            query = query.Where(filter);
        if (orderBy is not null)
            query = orderBy(query);
        return await query.ToListAsync(ct);
    }

    /// <summary>
    /// Fügt eine neue Entität hinzu (noch nicht gespeichert).
    /// </summary>
    public virtual async Task AddAsync(T entity, CancellationToken ct = default)
        => await Set.AddAsync(entity, ct);

    /// <summary>
    /// Fügt mehrere neue Entitäten hinzu (noch nicht gespeichert).
    /// </summary>
    public virtual async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default)
        => await Set.AddRangeAsync(entities, ct);

    /// <summary>
    /// Markiert eine Entität als geändert.
    /// </summary>
    public virtual void Update(T entity) => Set.Update(entity);

    /// <summary>
    /// Entfernt eine Entität.
    /// </summary>
    public virtual void Remove(T entity) => Set.Remove(entity);

    /// <summary>
    /// Entfernt mehrere Entitäten.
    /// </summary>
    public virtual void RemoveRange(IEnumerable<T> entities) => Set.RemoveRange(entities);
}

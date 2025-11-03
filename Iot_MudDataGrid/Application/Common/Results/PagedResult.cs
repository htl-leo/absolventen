namespace Application.Common.Results;

/// <summary>
/// Paginiertes Ergebnis als Result-kompatibler Wrapper.
/// </summary>
/// <typeparam name="T">Der Typ der Elemente in der Sammlung</typeparam>
public class PagedResult<T> : Result<PagedData<T>>
{
    private PagedResult(bool isSuccess, PagedData<T>? value, string? message, ResultType type)
        : base(isSuccess, value, message, type)
    {
    }

    /// <summary>
    /// Erstellt ein erfolgreiches paginiertes Result.
    /// </summary>
    public static PagedResult<T> Success(IReadOnlyCollection<T> items, int totalCount, int page, int pageSize, string? message = null)
        => new(true, new PagedData<T>(items, totalCount, page, pageSize), message, ResultType.Success);

    /// <summary>
    /// Erstellt ein PagedResult für Not Found.
    /// </summary>
    public new static PagedResult<T> NotFound(string? message = null)
        => new(false, default, message, ResultType.NotFound);

    /// <summary>
    /// Erstellt ein PagedResult für Validation Error.
    /// </summary>
    public new static PagedResult<T> ValidationError(string? message = null)
        => new(false, default, message, ResultType.ValidationError);

    /// <summary>
    /// Erstellt ein PagedResult für Conflict.
    /// </summary>
    public new static PagedResult<T> Conflict(string? message = null)
        => new(false, default, message, ResultType.Conflict);

    /// <summary>
    /// Erstellt ein PagedResult für Error.
    /// </summary>
    public new static PagedResult<T> Error(string? message = null)
        => new(false, default, message, ResultType.Error);
}

/// <summary>
/// Datenstruktur für paginierte Ergebnisse.
/// </summary>
/// <typeparam name="T">Der Typ der Elemente</typeparam>
public sealed record PagedData<T>(
    IReadOnlyCollection<T> Items, 
    int TotalCount, 
    int Page, 
    int PageSize)
{
    /// <summary>
    /// Berechnet die Gesamtzahl der Seiten.
    /// </summary>
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    /// <summary>
    /// Prüft, ob eine vorherige Seite existiert.
    /// </summary>
    public bool HasPreviousPage => Page > 1;

    /// <summary>
    /// Prüft, ob eine nächste Seite existiert.
    /// </summary>
    public bool HasNextPage => Page < TotalPages;
}

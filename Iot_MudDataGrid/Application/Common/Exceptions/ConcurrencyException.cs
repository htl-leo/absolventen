namespace Application.Common.Exceptions;

/// <summary>
/// Wird geworfen, wenn eine konkurrierende Aktualisierung festgestellt wurde.
/// </summary>
public sealed class ConcurrencyException(string message) : Exception(message)
{
}

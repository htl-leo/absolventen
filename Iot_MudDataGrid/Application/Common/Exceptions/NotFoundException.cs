namespace Application.Common.Exceptions;

/// <summary>
/// Wird geworfen, wenn eine angeforderte Ressource nicht gefunden wurde.
/// </summary>
public class NotFoundException(string message) : Exception(message)
{
}

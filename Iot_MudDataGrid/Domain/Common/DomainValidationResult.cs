namespace Domain.Common;

/// <summary>
/// Lightweight domain validation result with factory helpers.
/// </summary>
public sealed record DomainValidationResult(bool IsValid, string Property, string? ErrorMessage)
{
    public static DomainValidationResult Success(string property) => new(true, property, null);
    public static DomainValidationResult Failure(string property, string message) => new(false, property, message);
}

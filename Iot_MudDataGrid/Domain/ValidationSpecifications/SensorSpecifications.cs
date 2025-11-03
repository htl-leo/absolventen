using Domain.Common;

namespace Domain.ValidationSpecifications;
public static class SensorSpecifications
{
    public const int NameMinLength = 2;

    public static DomainValidationResult CheckLocation(string location) =>
        string.IsNullOrWhiteSpace(location)
            ? DomainValidationResult.Failure("Location", "Location darf nicht leer sein.")
            : DomainValidationResult.Success("Location");

    //public static ValidationResult CheckName(string name) =>
    //    string.IsNullOrWhiteSpace(name)
    //        ? ValidationResult.Failure("Name darf nicht leer sein.")
    //    : name.Trim().Length < NameMinLength
    //        ? ValidationResult.Failure($"Name muss mindestens {NameMinLength} Zeichen haben.")
    //        : ValidationResult.Success();

    public static DomainValidationResult CheckName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return DomainValidationResult.Failure("Name", "Name darf nicht leer sein.");
        }
        if (name.Trim().Length < NameMinLength)
        {
            return DomainValidationResult.Failure("Name", $"Name muss mindestens {NameMinLength} Zeichen haben.");
        }
        return DomainValidationResult.Success("Name");
    }


    public static DomainValidationResult CheckNameNotEqualLocation(string name, string location) =>
        string.Equals(name.Trim(), location.Trim(), StringComparison.OrdinalIgnoreCase)
            ? DomainValidationResult.Failure("Name", "Name darf nicht der Location entsprechen.")
            : DomainValidationResult.Success("Name");

    //public static IEnumerable<ValidationResult> ValidateSensorProperties(string location, string name)
    //{
    //    yield return CheckLocation(location);
    //    yield return CheckName(name);
    //    yield return CheckNameNotEqualLocation(name, location);
    //}


}

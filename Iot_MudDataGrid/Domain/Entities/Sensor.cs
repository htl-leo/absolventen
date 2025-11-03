using Domain.Common;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.ValidationSpecifications;

namespace Domain.Entities;

/// <summary>
/// Repräsentiert einen physischen Sensor an einem Standort.
/// </summary>
public class Sensor : BaseEntity
{
    /// <summary>
    /// Standort/Ort des Sensors, z.B. "Wohnzimmer".
    /// </summary>
    public string Location { get; private set; } = string.Empty;
    /// <summary>
    /// Anzeigename des Sensors, z.B. "Temperatur".
    /// </summary>  
    public string Name { get; private set; } = string.Empty;
    public ICollection<Measurement> Measurements { get; private set; } = default!;
    private Sensor() { } // Für EF Core notwendig (parameterloser Konstruktor)


    /// <summary>
    /// Asynchronously creates a new <see cref="Sensor"/> instance with the specified location and name.
    /// </summary>
    /// <remarks>This method validates the provided location and name both internally and externally using the
    /// specified <paramref name="uniquenessChecker"/>. The resulting <see cref="Sensor"/> instance is initialized with
    /// the trimmed values of the location and name.</remarks>
    /// <param name="location">The location of the sensor. Cannot be null or empty, and leading or trailing whitespace will be trimmed.</param>
    /// <param name="name">The name of the sensor. Cannot be null or empty, and leading or trailing whitespace will be trimmed.</param>
    /// <param name="uniquenessChecker">An implementation of <see cref="ISensorUniquenessChecker"/> used to ensure the sensor's uniqueness.</param>
    /// <param name="ct">An optional <see cref="CancellationToken"/> to observe while waiting for the operation to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the newly created <see
    /// cref="Sensor"/> instance.</returns>
    public static async Task<Sensor> CreateAsync(string location, string name, 
        ISensorUniquenessChecker uniquenessChecker, CancellationToken ct = default)
    {
        var trimmedLocation = (location ?? string.Empty).Trim();
        var trimmedName = (name ?? string.Empty).Trim();
        ValidateSensorProperties(trimmedLocation, trimmedName);
        await ValidateSensorUniqueness(0, trimmedLocation, trimmedName, uniquenessChecker, ct);
        return new Sensor { Location = trimmedLocation, Name = trimmedName };
    }
    /// <summary>
    /// Aktualisiert die Eigenschaften des Sensors.
    /// </summary>
    public async Task UpdateAsync(string location, string name, 
        ISensorUniquenessChecker uniquenessChecker, CancellationToken ct = default)
    {
        var trimmedLocation = (location ?? string.Empty).Trim();
        var trimmedName = (name ?? string.Empty).Trim();
        if (Location == trimmedLocation && Name == trimmedName)
            return; // Keine Änderung
        ValidateSensorProperties(trimmedLocation, trimmedName);
        await ValidateSensorUniqueness(Id, trimmedLocation, trimmedName, uniquenessChecker, ct);
        Location = trimmedLocation;
        Name = trimmedName;
    }
    public override string ToString() => $"{Location}_{Name}";
    public static void ValidateSensorProperties(string location, string name)
    {
        var validationResults = new List<DomainValidationResult>
        {
            SensorSpecifications.CheckLocation(location),
            SensorSpecifications.CheckName(name),
            SensorSpecifications.CheckNameNotEqualLocation(name, location)
        };
        foreach (var result in validationResults)
        {
            if (!result.IsValid)
            {
                throw new DomainValidationException(result.Property, result.ErrorMessage!);
            }
        }
    }
    public static async Task ValidateSensorUniqueness(int id, string location, string name,
            ISensorUniquenessChecker uniquenessChecker, CancellationToken ct = default)
    {
        if (!await uniquenessChecker.IsUniqueAsync(id, location, name, ct))
            throw new DomainValidationException("Uniqueness", "Ein Sensor mit der gleichen Location und dem gleichen Namen existiert bereits.");
    }


}

namespace Application.Features.Dtos;

/// <summary>
/// Datenübertragungsobjekt (DTO) für Sensoren inkl. Anzahl der Messungen.
/// Als Referenztyp (record class), um Nullability sauber zu erzwingen.
/// </summary>
public sealed record GetSensorWithNumberOfMeasurementsDto(int Id, string Location, string Name, int NumberOfMeasurements);

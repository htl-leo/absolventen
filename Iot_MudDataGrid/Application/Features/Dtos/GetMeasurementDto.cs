namespace Application.Features.Dtos;

/// <summary>
/// Datenübertragungsobjekt (DTO) für Messungen in der API-Antwort.
/// Als Referenztyp (record class), um Nullability sauber zu erzwingen.
/// </summary>
public sealed record GetMeasurementDto(int Id, int SensorId, double Value, DateTime Timestamp);

namespace Application.Features.Dtos;

/// <summary>
/// Datenübertragungsobjekt (DTO) für Sensoren ohne RowVersion/Concurrency-Token.
/// Als Referenztyp (record class), um Nullability sauber zu erzwingen.
/// </summary>
public sealed record GetSensorDto(int Id, string Location, string Name);

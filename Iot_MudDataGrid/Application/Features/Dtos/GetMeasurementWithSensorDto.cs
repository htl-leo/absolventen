namespace Application.Features.Dtos;

public sealed record GetMeasurementWithSensorDto(
    string SensorName,
    string Location,
    string Date,
    string Time,
    string Value
);

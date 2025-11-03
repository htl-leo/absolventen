using Application.Features.Dtos;
using MediatR;

namespace Application.Features.Measurements.Queries.GetAllMeasurementsStream;

public sealed record GetAllMeasurementsStreamQuery : IStreamRequest<GetMeasurementWithSensorDto>;

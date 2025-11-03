using Application.Common.Results;
using Application.Features.Dtos;
using MediatR;

namespace Application.Features.Sensors.Queries.GetSensorsWithCounts;

public readonly record struct GetSensorsWithCountsQuery : IRequest<Result<IReadOnlyCollection<GetSensorWithNumberOfMeasurementsDto>>>;

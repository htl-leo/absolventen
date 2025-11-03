using Application.Common.Results;
using Application.Features.Dtos;
using MediatR;

namespace Application.Features.Measurements.Queries.GetAllMeasurementsSimple;

public sealed record GetAllMeasurementsSimpleQuery : IRequest<Result<IReadOnlyCollection<GetMeasurementWithSensorDto>>>;

using Application.Common.Results;
using Application.Features.Dtos;
using MediatR;

namespace Application.Features.Sensors.Queries.GetAllSensors;

public readonly record struct GetAllSensorsQuery : IRequest<Result<IReadOnlyCollection<GetSensorDto>>>;

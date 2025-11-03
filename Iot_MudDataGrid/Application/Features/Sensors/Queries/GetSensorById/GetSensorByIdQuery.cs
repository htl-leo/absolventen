using Application.Common.Results;
using Application.Features.Dtos;
using MediatR;

namespace Application.Features.Sensors.Queries.GetSensorById;

public readonly record struct GetSensorByIdQuery(int Id) : IRequest<Result<GetSensorDto>>;

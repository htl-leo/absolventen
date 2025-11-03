using Application.Common.Results;
using Application.Features.Dtos;
using MediatR;

namespace Application.Features.Sensors.Commands.UpdateSensor;

public readonly record struct UpdateSensorCommand(int Id, string Location, string Name) : IRequest<Result<GetSensorDto>>;

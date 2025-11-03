using Application.Common.Results;
using MediatR;

namespace Application.Features.Sensors.Commands.DeleteSensor;

public readonly record struct DeleteSensorCommand(int Id) : IRequest<Result<bool>>;

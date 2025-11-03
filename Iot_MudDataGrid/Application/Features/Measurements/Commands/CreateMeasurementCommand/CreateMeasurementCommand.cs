using Application.Common.Results;
using Application.Features.Dtos;
using MediatR;

namespace Application.Features.Measurements.Commands.CreateMeasurementCommand;

public readonly record struct CreateMeasurementCommand(string Location, string Name, DateTime Timestamp, double Value)
    : IRequest<Result<GetMeasurementDto>>;

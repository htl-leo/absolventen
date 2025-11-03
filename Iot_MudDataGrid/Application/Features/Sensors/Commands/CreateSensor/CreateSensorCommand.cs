using Application.Common.Results;
using Application.Features.Dtos;
using MediatR;

namespace Application.Features.Sensors.Commands.CreateSensor;

// Validation über Result<T> ==> Resulttype ist Dto
public readonly record struct CreateSensorCommand(string Location, string Name) 
        : IRequest<Result<GetSensorDto>>;

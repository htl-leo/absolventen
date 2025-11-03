using Application.Common.Results;
using Application.Contracts;
using Application.Features.Dtos;
using Domain.Contracts;
using Domain.Entities;
using Mapster;
using MediatR;

namespace Application.Features.Measurements.Commands.CreateMeasurementCommand;

public class CreateMeasurementCommandHandler(IUnitOfWork uow, ISensorUniquenessChecker uniquenessChecker)
    : IRequestHandler<CreateMeasurementCommand, Result<GetMeasurementDto>>
{
    public async Task<Result<GetMeasurementDto>> Handle(CreateMeasurementCommand request, 
        CancellationToken cancellationToken)
    {
        // Sensor anhand Location/Name holen oder anlegen
        var sensor = await uow.Sensors.GetByLocationAndNameAsync(request.Location, request.Name,  cancellationToken);
        if (sensor is null)
        {
            //try
            //{
                //sensor = new Sensor(request.Location, request.Name);
                sensor = await Sensor.CreateAsync(request.Location, request.Name, uniquenessChecker, cancellationToken);
                await uow.Sensors.AddAsync(sensor, cancellationToken);
                // Save to get sensor.Id for FK
                await uow.SaveChangesAsync(cancellationToken);
            //}
            //catch (Exception ex)
            //{
            //    return Result<GetMeasurementDto>.ValidationError(ex.Message);
            //}
        }

        //if (request.Timestamp > DateTime.UtcNow || request.Timestamp < DateTime.UtcNow.AddHours(-1))
        //{
        //    return Result<GetMeasurementDto>.ValidationError("Invalid timestamp.");
        //}

        var measurement = Measurement.Create(sensor, request.Value, request.Timestamp);
        await uow.Measurements.AddAsync(measurement, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);

        return Result<GetMeasurementDto>.Created(measurement.Adapt<GetMeasurementDto>());
    }
}

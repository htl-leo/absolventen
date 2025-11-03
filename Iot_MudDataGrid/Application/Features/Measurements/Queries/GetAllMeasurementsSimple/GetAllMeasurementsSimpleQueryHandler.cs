using Application.Common.Results;
using Application.Contracts;
using Application.Features.Dtos;
using MediatR;

namespace Application.Features.Measurements.Queries.GetAllMeasurementsSimple;

public sealed class GetAllMeasurementsSimpleQueryHandler(IUnitOfWork uow) : IRequestHandler<GetAllMeasurementsSimpleQuery, Result<IReadOnlyCollection<GetMeasurementWithSensorDto>>>
{
    public async Task<Result<IReadOnlyCollection<GetMeasurementWithSensorDto>>> Handle(GetAllMeasurementsSimpleQuery request, CancellationToken cancellationToken)
    {
        var entities = await uow.Measurements.GetAllWithSensorAsync(cancellationToken);
        var dtos = entities
            .Select(m => new GetMeasurementWithSensorDto(
                SensorName: m.Sensor.Name,
                Location: m.Sensor.Location,
                Date: m.Timestamp.ToString("dd.MM.yyyy"),
                Time: m.Timestamp.ToString("HH:mm:ss"),
                Value: m.Value.ToString("F2")
            ))
            .ToArray();

        return Result<IReadOnlyCollection<GetMeasurementWithSensorDto>>.Success(dtos);
    }
}

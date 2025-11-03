using Application.Contracts;
using Application.Features.Dtos;
using MediatR;
using System.Runtime.CompilerServices;

namespace Application.Features.Measurements.Queries.GetAllMeasurementsStream;

public sealed class GetAllMeasurementsStreamQueryHandler(IUnitOfWork uow) : IStreamRequestHandler<GetAllMeasurementsStreamQuery, GetMeasurementWithSensorDto>
{
    public async IAsyncEnumerable<GetMeasurementWithSensorDto> Handle(GetAllMeasurementsStreamQuery request, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (var m in uow.Measurements.StreamAllWithSensorAsync(cancellationToken))
        {
            yield return new GetMeasurementWithSensorDto(
                SensorName: m.Sensor.Name,
                Location: m.Sensor.Location,
                Date: m.Timestamp.ToString("dd.MM.yyyy"),
                Time: m.Timestamp.ToString("HH:mm:ss"),
                Value: m.Value.ToString("F2")
            );
        }
    }
}

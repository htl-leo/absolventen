using Application.Common.Results;
using MediatR;
using Mapster;
using Application.Features.Dtos;
using Application.Contracts;

namespace Application.Features.Sensors.Queries.GetSensorsWithCounts;

public sealed class GetSensorsWithCountsQueryHandler(IUnitOfWork uow)
    : IRequestHandler<GetSensorsWithCountsQuery, Result<IReadOnlyCollection<GetSensorWithNumberOfMeasurementsDto>>>
{
    public async Task<Result<IReadOnlyCollection<GetSensorWithNumberOfMeasurementsDto>>> Handle(GetSensorsWithCountsQuery request, CancellationToken cancellationToken)
    {
        var records = await uow.Sensors.GetAllWithNumberOfMeasurementsAsync(cancellationToken);
        var dtos = records.Adapt<IReadOnlyCollection<GetSensorWithNumberOfMeasurementsDto>>();
        return Result<IReadOnlyCollection<GetSensorWithNumberOfMeasurementsDto>>.Success(dtos);
    }
}

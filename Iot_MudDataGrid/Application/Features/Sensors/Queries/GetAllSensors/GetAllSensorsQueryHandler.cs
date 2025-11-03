using Application.Common.Results;
using Application.Contracts;
using Application.Features.Dtos;
using Mapster;
using MediatR;

namespace Application.Features.Sensors.Queries.GetAllSensors;

public sealed class GetAllSensorsQueryHandler(IUnitOfWork uow) : IRequestHandler<GetAllSensorsQuery, 
    Result<IReadOnlyCollection<GetSensorDto>>>
{
    public async Task<Result<IReadOnlyCollection<GetSensorDto>>> Handle(GetAllSensorsQuery request, 
        CancellationToken cancellationToken)
    {
        var sensors = await uow.Sensors.GetAllAsync(
            orderBy: q => q.OrderBy(s => s.Location).ThenBy(s => s.Name), ct: cancellationToken);
        var dtos = sensors.Adapt<IReadOnlyCollection<GetSensorDto>>();
        return Result<IReadOnlyCollection<GetSensorDto>>.Success(dtos);
    }
}

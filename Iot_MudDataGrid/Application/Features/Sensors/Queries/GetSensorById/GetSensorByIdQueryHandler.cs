using Application.Common.Results;
using Application.Contracts;
using Application.Features.Dtos;
using Mapster;
using MediatR;

namespace Application.Features.Sensors.Queries.GetSensorById;

public sealed class GetSensorByIdQueryHandler(IUnitOfWork uow) : IRequestHandler<GetSensorByIdQuery, Result<GetSensorDto>>
{
    public async Task<Result<GetSensorDto>> Handle(GetSensorByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await uow.Sensors.GetByIdAsync(request.Id, cancellationToken);
        if (entity is null)
            return Result<GetSensorDto>.NotFound($"Sensor with ID {request.Id} not found.");
        return Result<GetSensorDto>.Success(entity.Adapt<GetSensorDto>());
    }
}

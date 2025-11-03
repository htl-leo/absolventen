using Application.Common.Results;
using Application.Contracts;
using Application.Features.Dtos;
using Mapster;
using MediatR;

namespace Application.Features.Measurements.Queries.GetBySensorIdPaged;

//public record GetMeasurementsBySensorIdPagedQuery(int SensorId, int Page, int PageSize)
//    : IRequest<PagedResult<GetMeasurementDto>>;

public sealed class GetMeasurementsBySensorIdPagedQueryHandler(IUnitOfWork uow)
    : IRequestHandler<GetMeasurementsBySensorIdPagedQuery, PagedResult<GetMeasurementDto>>
{
    public async Task<PagedResult<GetMeasurementDto>> Handle(GetMeasurementsBySensorIdPagedQuery request, 
        CancellationToken cancellationToken)
    {
        var page = request.Page < 1 ? 1 : request.Page;
        var pageSize = request.PageSize < 1 ? 50 : request.PageSize;
        var skip = (page - 1) * pageSize;
        var total = await uow.Measurements.CountBySensorIdAsync(request.SensorId, cancellationToken);
        var items = await uow.Measurements.GetBySensorIdPagedAsync(request.SensorId, skip, pageSize, cancellationToken);
        var dtos = items.Select(i => new GetMeasurementDto(i.Id, i.SensorId, i.Value, i.Timestamp)).ToList();
        return PagedResult<GetMeasurementDto>.Success(dtos, total, page, pageSize);
    }
}

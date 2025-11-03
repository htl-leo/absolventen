using Application.Common.Results;
using Application.Features.Dtos;
using MediatR;

namespace Application.Features.Measurements.Queries.GetBySensorIdPaged;

public record GetMeasurementsBySensorIdPagedQuery(int SensorId, int Page, int PageSize)
: IRequest<PagedResult<GetMeasurementDto>>;

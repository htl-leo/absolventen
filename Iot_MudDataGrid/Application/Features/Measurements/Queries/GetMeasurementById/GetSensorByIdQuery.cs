using Application.Common.Results;
using Application.Features.Dtos;
using MediatR;

namespace Application.Features.Measurements.Queries.GetMeasurementById;

public readonly record struct GetMeasurementByIdQuery(int Id) : IRequest<Result<GetMeasurementDto>>;

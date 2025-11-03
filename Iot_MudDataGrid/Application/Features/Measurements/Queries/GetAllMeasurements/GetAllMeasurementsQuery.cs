using Application.Common.Results;
using Application.Features.Dtos;
using MediatR;

namespace Application.Features.Measurements.Queries.GetAllMeasurements;

public record GetAllMeasurementsQuery : IRequest<Result<IReadOnlyCollection<GetMeasurementDto>>>;

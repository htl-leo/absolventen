using Application.Common.Results;
using Application.Contracts;
using Application.Features.Dtos;
using Mapster;
using MediatR;

namespace Application.Features.Measurements.Queries.GetMeasurementById;

/// <summary>
/// Handler zum Abrufen einer Messung per Id.
/// </summary>
public sealed class GetMeasurementByIdQueryHandler(IUnitOfWork uow) : IRequestHandler<GetMeasurementByIdQuery, Result<GetMeasurementDto>>
{
    /// <summary>
    /// Lädt die Messung und mappt sie auf ein DTO. Gibt null zurück, wenn nicht vorhanden.
    /// </summary>
    public async Task<Result<GetMeasurementDto>> Handle(GetMeasurementByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await uow.Measurements.GetByIdAsync(request.Id, cancellationToken);
        if (entity is null)
            return Result<GetMeasurementDto>.NotFound($"Measurement with ID {request.Id} not found.");
        return Result<GetMeasurementDto>.Success(entity.Adapt<GetMeasurementDto>());
    }
}

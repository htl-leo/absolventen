using Application.Common.Results;
using Application.Contracts;
using Application.Features.Dtos;
using Mapster;
using MediatR;

namespace Application.Features.Measurements.Queries.GetAllMeasurements;

/// <summary>
/// Query-Handler zum Abrufen aller Messungen.
/// Hinweis: In echten Anwendungen sollte hier Pagination eingesetzt werden.
/// </summary>
public sealed class GetAllMeasurementsQueryHandler(IUnitOfWork uow) : IRequestHandler<GetAllMeasurementsQuery, Result<IReadOnlyCollection<GetMeasurementDto>>>
{
    public async Task<Result<IReadOnlyCollection<GetMeasurementDto>>> Handle(GetAllMeasurementsQuery request, CancellationToken cancellationToken)
    {
        // Alle Mess-Entities laden und mittels Mapster in DTOs transformieren
        var entities = await uow.Measurements.GetAllAsync( ct: cancellationToken);
        var dtos = entities.Adapt<IReadOnlyCollection<GetMeasurementDto>>();
        return Result<IReadOnlyCollection<GetMeasurementDto>>.Success(dtos);
    }
}

using Application.Contracts;
using Domain.Contracts;

namespace Application.Services;

public class SensorUniquenessChecker(IUnitOfWork uow) : ISensorUniquenessChecker
{
    public async Task<bool> IsUniqueAsync(int id, string location, string name, CancellationToken ct = default)
    {
        var existing = await uow.Sensors.GetByLocationAndNameAsync(location, name, ct);
        return existing is null || existing.Id == id;
    }
}

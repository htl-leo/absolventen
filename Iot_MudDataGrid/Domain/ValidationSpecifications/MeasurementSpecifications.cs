using Domain.Common;

namespace Domain.ValidationSpecifications;
public static class MeasurementSpecifications
{
    public static DomainValidationResult CheckSensorId(int id) =>
        id <= 0
            ? DomainValidationResult.Failure("SensorId", "SensorId muss positiv sein.")
            : DomainValidationResult.Success("SensorId");

}

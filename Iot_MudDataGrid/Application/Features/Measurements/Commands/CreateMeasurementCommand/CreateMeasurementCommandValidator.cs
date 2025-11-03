using FluentValidation;

namespace Application.Features.Measurements.Commands.CreateMeasurementCommand;

public class CreateMeasurementCommandValidator : AbstractValidator<CreateMeasurementCommand>
{
    public CreateMeasurementCommandValidator()
    {
        RuleFor(x => x.Timestamp)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Timestamp cannot be in the future")
            .GreaterThanOrEqualTo(DateTime.UtcNow.AddHours(-1))
            .WithMessage("Timestamp cannot be older than 1 hour");

    }
}

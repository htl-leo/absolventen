using FluentValidation;

namespace Application.Features.Sensors.Commands.UpdateSensor;

public class UpdateSensorCommandValidator : AbstractValidator<UpdateSensorCommand>
{
    public UpdateSensorCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.Location)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(2)
            .Must((cmd, name) => !string.Equals(name, cmd.Location, StringComparison.OrdinalIgnoreCase))
            .WithMessage("Name must not equal Location.");
    }
}

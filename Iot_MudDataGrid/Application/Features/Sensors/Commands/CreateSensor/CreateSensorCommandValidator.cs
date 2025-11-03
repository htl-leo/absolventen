using FluentValidation;

namespace Application.Features.Sensors.Commands.CreateSensor;

public class CreateSensorValidator : AbstractValidator<CreateSensorCommand>
{
    public CreateSensorValidator()
    {
        //RuleFor(x => x.Location)
        //    .NotEmpty();

        //RuleFor(x => x.Name)
        //    .NotEmpty()
        //    .MinimumLength(2)
        //    .Must((cmd, name) => !string.Equals(name, cmd.Location, StringComparison.OrdinalIgnoreCase))
        //    .WithMessage("Name must not equal Location.");
    }
}

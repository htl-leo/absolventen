using Application.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Application.Pipeline;
using Domain.Contracts;
using Application.Contracts;

namespace Application;

/// <summary>
/// Erweiterungsmethoden für DI-Registrierung der Infrastrukturdienste.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registriert DbContext, Repositories, UnitOfWork, CSV-Reader und Seeder.
    /// </summary>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // CQRS + MediatR + FluentValidation
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(IUnitOfWork).Assembly); // Application-Assembly
        });
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddValidatorsFromAssembly(typeof(IUnitOfWork).Assembly);

        // Domain Services
        services.AddScoped<ISensorUniquenessChecker, SensorUniquenessChecker>();

        return services;
    }
}


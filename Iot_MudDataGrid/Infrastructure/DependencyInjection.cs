using Application.Contracts;
using Application.Contracts.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure;

/// <summary>
/// Erweiterungsmethoden für DI-Registrierung der Infrastrukturdienste.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registriert DbContext, Repositories, UnitOfWork, CSV-Reader und Seeder.
    /// </summary>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string csvPath, 
                string connectionString)
    {
        services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(connectionString));

        // Repositories und UoW (Scoped: pro HTTP-Request eine Instanz)
        services.AddScoped<ISensorRepository, SensorRepository>();
        services.AddScoped<IMeasurementRepository, MeasurementRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Hosted Service zum initialen Datenimport beim Start der Anwendung
        // CsvPath via Options bereitstellen und HostedService normal registrieren
        services.Configure<StartupDataSeederOptions>(o => o.CsvPath = csvPath);
        services.AddHostedService<StartupDataSeeder>();

        return services;
    }
}

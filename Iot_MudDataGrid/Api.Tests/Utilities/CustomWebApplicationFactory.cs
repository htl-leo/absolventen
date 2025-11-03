using Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Persistence.Repositories;
using Application.Services;
using FluentValidation;
using Domain.Contracts;
using Application.Contracts.Repositories;
using Application.Contracts;

namespace Api.Tests.Utilities;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("SkipInfrastructure", "true");

        builder.ConfigureServices(services =>
        {
            // Remove previously registered DbContext (if any)
            var toRemove = services.Where(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>) || d.ServiceType == typeof(AppDbContext)).ToList();
            foreach (var d in toRemove) services.Remove(d);

            services.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase("ApiTestsDb"));

            // Register repositories & UoW (mimic Infrastructure.AddInfrastructure, but without SQL / seeder)
            services.AddScoped<ISensorRepository, SensorRepository>();
            services.AddScoped<IMeasurementRepository, MeasurementRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ISensorUniquenessChecker, SensorUniquenessChecker>();

            // Add Application layer MediatR + Validators if not already (idempotent)
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IUnitOfWork).Assembly));
            services.AddValidatorsFromAssembly(typeof(IUnitOfWork).Assembly);

            // Build provider & ensure db created
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.EnsureCreated();
        });
    }
}

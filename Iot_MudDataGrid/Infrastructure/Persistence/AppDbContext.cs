using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

/// <summary>
/// EF Core DbContext. Verwaltet die Verbindung zur Datenbank und das Mapping der Entitäten.
/// </summary>
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Tabelle/DbSet für Sensoren.
    /// </summary>
    public DbSet<Sensor> Sensors => Set<Sensor>();

    /// <summary>
    /// Tabelle/DbSet für Messungen.
    /// </summary>
    public DbSet<Measurement> Measurements => Set<Measurement>();

    /// <summary>
    /// Fluent-API Konfigurationen für EF Core.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Sensor>(sensor =>
        {
            sensor.Property(s => s.Location).HasMaxLength(100).IsRequired();
            sensor.Property(s => s.Name).HasMaxLength(100).IsRequired();

            // RowVersion für Optimistic Concurrency
            sensor.Property(s => s.RowVersion).IsRowVersion();
            // Uniqueness constraint (Location + Name)
            sensor.HasIndex(s => new { s.Location, s.Name })
                  .IsUnique()
                  .HasDatabaseName("UX_Sensors_Location_Name");
        });

        modelBuilder.Entity<Measurement>(measurement =>
        {
            // Index für häufige Abfragen nach Sensor und Zeitpunkt
            measurement.HasIndex(m => new { m.SensorId, m.Timestamp });
            // Beziehung: Jede Messung gehört zu genau einem Sensor (Cascade Delete)
            measurement.HasOne(m => m.Sensor)
                .WithMany(s => s.Measurements)
                .HasForeignKey(m => m.SensorId)
                .OnDelete(DeleteBehavior.Cascade);
            // RowVersion für Optimistic Concurrency
            measurement.Property(m => m.RowVersion).IsRowVersion();
        });
    }
}

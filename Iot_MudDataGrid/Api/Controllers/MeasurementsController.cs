using Api.Extensions;
using Application.Common.Results;
using Application.Features.Dtos;
using Application.Features.Measurements.Commands.CreateMeasurementCommand;
using Application.Features.Measurements.Queries.GetAllMeasurements;
using Application.Features.Measurements.Queries.GetBySensorIdPaged;
using Application.Features.Measurements.Queries.GetMeasurementById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.ObjectModel;

namespace Api.Controllers;

/// <summary>
/// Endpunkte für Messungen.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class MeasurementsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Gibt alle Messungen zurück. In echten Apps besser paginieren!
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GetMeasurementDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetAllMeasurementsQuery(), ct);
        return result.ToActionResult(this);
    }


    /// <summary>
    /// Liefert einen Messwert per Id, falls vorhanden.
    /// Sonst 404 Not Found.
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(GetMeasurementDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetMeasurementByIdQuery(id), ct);
        return result.ToActionResult(this);
    }


    /// <summary>
    /// Paginierte Messungen für einen Sensor (über SensorId).
    /// </summary>
    [HttpGet("bysensorid/{sensorId:int}")]
    [ProducesResponseType(typeof(PagedData<GetMeasurementDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBySensorIdPaged(int sensorId, 
            [FromQuery] int page = 1, [FromQuery] int pageSize = 50, CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetMeasurementsBySensorIdPagedQuery(sensorId, page, pageSize), ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Fügt eine Messung für den Sensor (Location/Name) hinzu. Legt den Sensor ggf. an.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(GetMeasurementDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Add([FromBody] CreateMeasurementCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return result.ToActionResult(this, createdAtAction: nameof(GetById), routeValues: new { id = result.Value?.Id });
    }
}

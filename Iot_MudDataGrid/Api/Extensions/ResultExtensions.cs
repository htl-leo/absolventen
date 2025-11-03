using Application.Common.Results;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Api.Extensions;

/*
Diese ResultExtensions-Klasse ist ein architektonisches Bindeglied zwischen deiner Anwendungsschicht (Application) und 
der API-Schicht (Presentation).
Sie bringt in Summe vier große Vorteile: 
    saubere Schichtentrennung, 
    weniger Boilerplate, 
    konsistente HTTP-Antworten (DRY) und 
    bessere Testbarkeit.
*/

public static class ResultExtensions
{
        /// <summary>
        /// Konvertiert ein generisches Result<T> aus der Anwendungsschicht in ein passendes IActionResult.
        /// </summary>
        public static IActionResult ToActionResult<T>(this Result<T> result,
                ControllerBase controller,
                string? createdAtAction = null,
                object? routeValues = null)
        {
            return result.Type switch
            {
                ResultType.Success => controller.Ok(result.Value),
                // Für Created nach Möglichkeit immer CreatedAtAction verwenden, damit der Location-Header gesetzt wird
                ResultType.Created => createdAtAction is not null
                    ? controller.CreatedAtAction(createdAtAction, routeValues, result.Value)
                    : controller.StatusCode(StatusCodes.Status201Created, result.Value),
                ResultType.NoContent => controller.NoContent(),
                ResultType.NotFound => controller.NotFound(result.Message),
                ResultType.ValidationError => controller.BadRequest(result.Message),
                ResultType.Conflict => controller.Conflict(result.Message),
                _ => controller.Problem(
                    detail: result.Message ?? "An unexpected error occurred.",
                    statusCode: 500
                )
            };
        }

        // CreatedAtAction(nameof(GetById), new { id = created.Id }, created);

        /// <summary>
        /// Konvertiert ein nicht-generisches Result in ein passendes IActionResult.
        /// </summary>
        public static IActionResult ToActionResult(this Result result, 
                ControllerBase controller,
                string? createdAtAction = null,
                object? routeValues = null)
        {
            return result.Type switch
            {
                // Ohne Value keinen Body zurückgeben
                ResultType.Success => controller.Ok(),
                // Für Created stets CreatedAtAction nutzen, Body weglassen
                ResultType.Created => createdAtAction is not null
                    ? controller.CreatedAtAction(createdAtAction, routeValues, null)
                    : controller.StatusCode(StatusCodes.Status201Created),
                ResultType.NoContent => controller.NoContent(),
                ResultType.NotFound => controller.NotFound(result.Message),
                ResultType.ValidationError => controller.BadRequest(result.Message),
                ResultType.Conflict => controller.Conflict(result.Message),
                _ => controller.Problem(
                    detail: result.Message ?? "An unexpected error occurred.",
                    statusCode: 500
                )
            };
        }
}

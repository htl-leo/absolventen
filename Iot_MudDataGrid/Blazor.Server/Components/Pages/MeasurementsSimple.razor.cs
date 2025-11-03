using Application.Features.Dtos;
using Application.Features.Measurements.Queries.GetAllMeasurementsSimple;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace Blazor.Server.Components.Pages;

public partial class MeasurementsSimple : ComponentBase
{
    [Inject] private IMediator Mediator { get; set; } = default!;

    private IReadOnlyCollection<GetMeasurementWithSensorDto>? _items;
    private string? _error;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var result = await Mediator.Send(new GetAllMeasurementsSimpleQuery());
            if (result.IsSuccess && result.Value is not null)
            {
                _items = result.Value;
            }
            else
            {
                _error = result.Message ?? "Unknown error";
            }
        }
        catch (Exception ex)
        {
            _error = ex.Message;
        }
    }
}

using Application.Features.Dtos;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace Blazor.Server.Components.Pages;

public partial class MeasurementsStream : ComponentBase, IDisposable
{
    private readonly List<GetMeasurementWithSensorDto> _items = [];
    private bool _isLoading = true;
    private string? _error;
    private readonly CancellationTokenSource _cts = new();
    private bool _disposed;

    [Inject] private IMediator Mediator { get; set; } = default!;

    protected override Task OnInitializedAsync()
    {
        _ = LoadStreamAsync(_cts.Token);
        return Task.CompletedTask;
    }

    private async Task LoadStreamAsync(CancellationToken ct)
    {
        try
        {
            int i = 0;
            await foreach (var dto in Mediator.CreateStream(new Application.Features.Measurements.Queries.GetAllMeasurementsStream.GetAllMeasurementsStreamQuery(), ct))
            {
                _items.Add(dto);
                if (_isLoading && _items.Count > 0)
                    _isLoading = false;

                if (++i % 50 == 0)
                {
                    await InvokeAsync(StateHasChanged);
                    Console.WriteLine($"Received measurement: {dto.SensorName} at {dto.Date} {dto.Time} with value {dto.Value}");
                }
            }
            await InvokeAsync(StateHasChanged);
        }
        catch (OperationCanceledException)
        {
            // ignore
        }
        catch (Exception ex)
        {
            _error = ex.Message;
            await InvokeAsync(StateHasChanged);
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;
        if (disposing)
        {
            _cts.Cancel();
            _cts.Dispose();
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}

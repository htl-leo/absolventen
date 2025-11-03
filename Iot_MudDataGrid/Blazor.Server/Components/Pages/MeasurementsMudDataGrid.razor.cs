using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Results;
using Application.Features.Dtos;
using Application.Features.Measurements.Queries.GetBySensorIdPaged;
using Application.Features.Sensors.Queries.GetAllSensors;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Blazor.Server.Components.Pages;

public partial class MeasurementsMudDataGrid : ComponentBase
{
    [Inject] private IMediator Mediator { get; set; } = default!;

    private MudDataGrid<GetMeasurementDto>? _grid;

    private string? _selectedLocation;
    private string? _selectedName;

    private readonly HashSet<string> _locations = [];
    private readonly List<string> _availableNames = [];

    private readonly List<(int Id, string Location, string Name)> _allSensors = [];

    private string? _error;

    // Track paging info for custom pager content
    private int _totalItems;
    private int _pageSize = 20; // default matches RowsPerPage
    private int _currentPage;   // 0-based as provided by MudDataGrid

    protected override async Task OnInitializedAsync()
    {
        await LoadSensorsAsync();
    }

    private async Task LoadSensorsAsync()
    {
        var result = await Mediator.Send(new GetAllSensorsQuery());
        if (!result.IsSuccess || result.Value is null)
        {
            _error = result.Message ?? "Fehler beim Laden der Sensoren";
            return;
        }

        _allSensors.Clear();
        _locations.Clear();
        foreach (var s in result.Value)
        {
            _allSensors.Add((s.Id, s.Location, s.Name));
            _locations.Add(s.Location);
        }
        UpdateAvailableNames();
    }

    private void UpdateAvailableNames()
    {
        _availableNames.Clear();
        if (!string.IsNullOrWhiteSpace(_selectedLocation))
        {
            _availableNames.AddRange(_allSensors.Where(s => s.Location == _selectedLocation)
                                                .Select(s => s.Name)
                                                .Distinct()
                                                .OrderBy(n => n));
        }
        if (!_availableNames.Contains(_selectedName ?? string.Empty))
        {
            _selectedName = null;
        }
    }

    private async Task OnLocationChanged(string? newLocation)
    {
        _selectedLocation = newLocation;
        UpdateAvailableNames();
        _currentPage = 0;
        _totalItems = 0;
        if (_grid is not null)
            await _grid.ReloadServerData();
        StateHasChanged();
    }

    private async Task OnNameChanged(string? newName)
    {
        _selectedName = newName;
        _currentPage = 0;
        _totalItems = 0;
        if (_grid is not null)
            await _grid.ReloadServerData();
        StateHasChanged();
    }

    private async Task<GridData<GetMeasurementDto>> LoadServerData(GridState<GetMeasurementDto> state)
    {
        try
        {
            _currentPage = state.Page; // 0-based
            _pageSize = state.PageSize > 0 ? state.PageSize : _pageSize;

            // If no selection made, return empty data
            if (string.IsNullOrWhiteSpace(_selectedLocation) || string.IsNullOrWhiteSpace(_selectedName))
            {
                _totalItems = 0;
                return new GridData<GetMeasurementDto> { Items = [], TotalItems = 0 };
            }

            var sensorId = _allSensors.FirstOrDefault(s => s.Location == _selectedLocation && s.Name == _selectedName).Id;
            if (sensorId == 0)
            {
                _totalItems = 0;
                return new GridData<GetMeasurementDto> { Items = [], TotalItems = 0 };
            }

            var page = state.Page + 1; // server is 1-based
            var pageSize = state.PageSize > 0 ? state.PageSize : 20;

            var result = await Mediator.Send(new GetMeasurementsBySensorIdPagedQuery(sensorId, page, pageSize));
            if (!result.IsSuccess || result.Value is null)
            {
                _error = result.Message ?? "Fehler beim Laden der Messwerte";
                _totalItems = 0;
                return new GridData<GetMeasurementDto> { Items = [], TotalItems = 0 };
            }

            var data = result.Value;
            _totalItems = data.TotalCount;
            return new GridData<GetMeasurementDto>
            {
                Items = data.Items,
                TotalItems = data.TotalCount
            };
        }
        catch (System.Exception ex)
        {
            _error = ex.Message;
            _totalItems = 0;
            return new GridData<GetMeasurementDto> { Items = [], TotalItems = 0 };
        }
    }
}

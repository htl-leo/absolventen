using System.Net;
using System.Net.Http.Json;
using Application.Features.Sensors.Commands.CreateSensor;
using Api.Tests.Utilities;
using FluentAssertions;
using Xunit;
using Application.Features.Sensors.Commands.UpdateSensor;
using Application.Features.Dtos;

namespace Api.Tests.Sensors;

public class SensorsEndpointTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task CreateSensor_Returns201_AndBody()
    {
        var cmd = new CreateSensorCommand("Wohnzimmer", "Temp");
        var response = await _client.PostAsJsonAsync("/api/sensors", cmd);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var dto = await response.Content.ReadFromJsonAsync<GetSensorDto>();
        dto.Should().NotBeNull();
        dto!.Location.Should().Be("Wohnzimmer");
        dto.Name.Should().Be("Temp");
    }

    [Fact]
    public async Task CreateSensor_Duplicate_Returns400()
    {
        var cmd = new CreateSensorCommand("Keller", "Temp");
        var response = await _client.PostAsJsonAsync("/api/sensors", cmd);  // erstes mal
        response.StatusCode.Should().Be(HttpStatusCode.Created); // OK
        response = await _client.PostAsJsonAsync("/api/sensors", cmd);  // duplikat
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest); // Validator triggered
    }

    [Fact]
    public async Task GetAll_ReturnsList()
    {
        await _client.PostAsJsonAsync("/api/sensors", new CreateSensorCommand("A", "T1"));
        await _client.PostAsJsonAsync("/api/sensors", new CreateSensorCommand("B", "T2"));

        var response = await _client.GetAsync("/api/sensors");
        response.EnsureSuccessStatusCode();
        var list = await response.Content.ReadFromJsonAsync<List<GetSensorDto>>();
        list.Should().NotBeNull();
        list!.Count.Should().BeGreaterOrEqualTo(2);
    }

    [Fact]
    public async Task GetById_NotFound_Returns404()
    {
        var response = await _client.GetAsync("/api/sensors/999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateSensor_Works()
    {
        var create = new CreateSensorCommand("Raum1", "Temp");
        var createdResp = await _client.PostAsJsonAsync("/api/sensors", create);
        var created = await createdResp.Content.ReadFromJsonAsync<GetSensorDto>();
        var updateDto = new UpdateSensorCommand {Id=created!.Id, Location = "Raum2", Name = "Temp2" };
        var updateResp = await _client.PutAsJsonAsync($"/api/sensors/{created!.Id}", updateDto);
        updateResp.EnsureSuccessStatusCode();
        var updated = await updateResp.Content.ReadFromJsonAsync<GetSensorDto>();
        updated!.Location.Should().Be("Raum2");
        updated.Name.Should().Be("Temp2");
    }

    [Fact]
    public async Task DeleteSensor_Works()
    {
        var create = new CreateSensorCommand("DelLoc", "DelName");
        var createdResp = await _client.PostAsJsonAsync("/api/sensors", create);
        var created = await createdResp.Content.ReadFromJsonAsync<GetSensorDto>();
        var delResp = await _client.DeleteAsync($"/api/sensors/{created!.Id}");
        delResp.StatusCode.Should().Be(HttpStatusCode.NoContent);
        var getResp = await _client.GetAsync($"/api/sensors/{created.Id}");
        getResp.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}

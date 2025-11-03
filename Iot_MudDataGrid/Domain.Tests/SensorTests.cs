using Domain.Contracts;
using Domain.Entities;
using Domain.Exceptions;
using Xunit;

namespace Domain.Tests;

public class SensorTests
{
    private class FakeUniquenessChecker(bool unique) : ISensorUniquenessChecker
    {
        public Task<bool> IsUniqueAsync(int id, string location, string name, CancellationToken ct = default) => Task.FromResult(unique);
    }

    [Fact]
    public async Task CreateAsync_Succeeds_WithValidData()
    {
        var checker = new FakeUniquenessChecker(true);
        var sensor = await Sensor.CreateAsync("Wohnzimmer", "Temperatur", checker);
        Assert.Equal("Wohnzimmer", sensor.Location);
        Assert.Equal("Temperatur", sensor.Name);
    }

    [Theory]
    [InlineData(null, "Temp", "Location darf nicht leer sein.")]
    [InlineData("", "Temp", "Location darf nicht leer sein.")]
    [InlineData("Wohnzimmer", "", "Name darf nicht leer sein.")]
    [InlineData("Wohnzimmer", "T", "Name muss mindestens 2 Zeichen haben.")]
    [InlineData("Wohnzimmer", "Wohnzimmer", "Name darf nicht der Location entsprechen.")]
    public async Task CreateAsync_InvalidRules_Throws(string? location, string? name, string expectedMessage)
    {
        var checker = new FakeUniquenessChecker(true);
        var ex = await Assert.ThrowsAsync<DomainValidationException>(() => Sensor.CreateAsync(location ?? string.Empty, name ?? string.Empty, checker));
        Assert.Equal(expectedMessage, ex.Message);
    }

    [Fact]
    public async Task CreateAsync_Duplicate_Throws()
    {
        var checker = new FakeUniquenessChecker(false);
        var ex = await Assert.ThrowsAsync<DomainValidationException>(() => Sensor.CreateAsync("Wohnzimmer", "Temperatur", checker));
        Assert.Equal("Ein Sensor mit der gleichen Location und dem gleichen Namen existiert bereits.", ex.Message);
    }

    [Fact]
    public async Task UpdateAsync_ChangesValues_WhenUnique()
    {
        var checker = new FakeUniquenessChecker(true);
        var sensor = await Sensor.CreateAsync("A", "Bz", checker); // Name needs 2 chars
        await sensor.UpdateAsync("Keller", "Feuchte", checker);
        Assert.Equal("Keller", sensor.Location);
        Assert.Equal("Feuchte", sensor.Name);
    }

    [Fact]
    public async Task UpdateAsync_Duplicate_Throws()
    {
        var checkerUnique = new FakeUniquenessChecker(true);
        var sensor = await Sensor.CreateAsync("Keller", "Temp", checkerUnique);
        var checkerDuplicate = new FakeUniquenessChecker(false);
        var ex = await Assert.ThrowsAsync<DomainValidationException>(() => sensor.UpdateAsync("Wohnzimmer", "Temp", checkerDuplicate));
        Assert.Equal("Ein Sensor mit der gleichen Location und dem gleichen Namen existiert bereits.", ex.Message);
    }

    [Fact]
    public async Task UpdateAsync_SameValues_NoUniquenessCheckNeeded()
    {
        bool uniquenessCalled = false;
        var checker = new CallbackUniquenessChecker(() => uniquenessCalled = true);
        var sensor = await Sensor.CreateAsync("Wohnzimmer", "Temp", new FakeUniquenessChecker(true));
        await sensor.UpdateAsync("Wohnzimmer", "Temp", checker); // same values
        Assert.False(uniquenessCalled); // uniqueness check not needed because values unchanged
    }

    private class CallbackUniquenessChecker(Action callback) : ISensorUniquenessChecker
    {
        public Task<bool> IsUniqueAsync(int id, string location, string name, CancellationToken ct = default)
        {
            callback();
            return Task.FromResult(true);
        }
    }
}

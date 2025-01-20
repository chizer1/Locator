using System.Linq;
using Locator;
using Locator.Domain;
using LocatorTests.Fixtures;

namespace LocatorTests;

[Collection("Locator")]
public class DatabaseServerTests
{
    private readonly Locator.Locator _locator;

    public DatabaseServerTests(LocatorFixture locatorFixture)
    {
        _locator = locatorFixture.Locator;
    }

    [Fact]
    public async Task AddMultipleDatabaseServersAndSearchByKeyWord()
    {
        var databaseServerName = StringUtilities.RandomString(10);
        var databaseServerLocation = StringUtilities.RandomString(10);
        var databaseServerId = await _locator.AddDatabaseServer(
            databaseServerName,
            databaseServerLocation
        );

        var databaseServerName2 = StringUtilities.RandomString(10);
        var databaseServerLocation2 = StringUtilities.RandomString(10);
        var databaseServerId2 = await _locator.AddDatabaseServer(
            databaseServerName2,
            databaseServerLocation2
        );

        var databaseServers = (await _locator.GetDatabaseServers())
            .Where(x => x.Name == databaseServerName)
            .ToList();

        Assert.True(databaseServers.Count() == 1);
        Assert.Equal(databaseServerName, databaseServers[0].Name);
        Assert.Equal(databaseServerLocation, databaseServers[0].IpAddress);
    }

    [Fact]
    public async Task AddAndDeleteDatabaseServer()
    {
        var databaseServerName = StringUtilities.RandomString(10);
        var databaseServerLocation = StringUtilities.RandomString(10);
        var databaseServerId = await _locator.AddDatabaseServer(
            databaseServerName,
            databaseServerLocation
        );

        await _locator.DeleteDatabaseServer(databaseServerId);
        var databaseServer = (await _locator.GetDatabaseServers())
            .Where(x => x.Name == databaseServerName)
            .ToList();
        Assert.True(databaseServer.Count() == 0);
    }

    [Fact]
    public async Task AddAndUpdateDatabaseServer()
    {
        var databaseServerName = StringUtilities.RandomString(10);
        var databaseServerLocation = StringUtilities.RandomString(10);
        var databaseServerId = await _locator.AddDatabaseServer(
            databaseServerName,
            databaseServerLocation
        );

        var databaseServerName2 = StringUtilities.RandomString(10);
        var databaseServerLocation2 = StringUtilities.RandomString(10);
        await _locator.UpdateDatabaseServer(
            databaseServerId,
            databaseServerName2,
            databaseServerLocation2
        );

        var databaseServers = (await _locator.GetDatabaseServers())
            .Where(x => x.Name == databaseServerName2)
            .ToList();

        Assert.True(databaseServers.Count() == 1);
        Assert.Equal(databaseServerName2, databaseServers[0].Name);
        Assert.Equal(databaseServerLocation2, databaseServers[0].IpAddress);
    }
}

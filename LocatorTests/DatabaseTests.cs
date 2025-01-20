using System.Linq;
using Locator;
using Locator.Domain;
using LocatorTests.Fixtures;

namespace LocatorTests;

[Collection("Locator")]
public class DatabaseTests
{
    private Locator.Locator _locator;
    private readonly int _databaseServerID;
    private readonly byte _databaseTypeId;

    public DatabaseTests(LocatorFixture locatorFixture)
    {
        _locator = locatorFixture.Locator;
        _databaseServerID = _locator.AddDatabaseServer("DatabaseServer", "localhost").Result;
        _databaseTypeId = (byte)(_locator.AddDatabaseType("DatabaseType").Result);
    }

    [Fact]
    public async Task AddMultipleDatabasesAndSearchByKeyWord()
    {
        var databaseName = $"[{StringUtilities.RandomString(10)}]";
        var databaseUser = $"[{StringUtilities.RandomString(10)}]";
        var databaseId = await _locator.AddDatabase(
            databaseName,
            databaseUser,
            _databaseServerID,
            _databaseTypeId,
            Status.Active,
            false,
            true
        );

        var databaseName2 = $"[{StringUtilities.RandomString(10)}]";
        var databaseUser2 = $"[{StringUtilities.RandomString(10)}]";
        var databaseId2 = await _locator.AddDatabase(
            databaseName2,
            databaseUser2,
            _databaseServerID,
            _databaseTypeId,
            Status.Active,
            false,
            true
        );

        var databases = (
            await _locator.GetDatabases(databaseName.Substring(0, 5), 1, 25)
        ).Items.ToList();
        Assert.True(databases.Count() == 1);
        Assert.Equal(databaseName, databases[0].Name);
    }

    [Fact]
    public async Task AddAndDeleteDatabase()
    {
        var databaseName = $"[{StringUtilities.RandomString(10)}]";
        var databaseUser = $"[{StringUtilities.RandomString(10)}]";
        var databaseId = await _locator.AddDatabase(
            databaseName,
            databaseUser,
            _databaseServerID,
            _databaseTypeId,
            Status.Active,
            false,
            true
        );

        await _locator.DeleteDatabase(databaseId);

        var database = await _locator.GetDatabases(databaseName.Substring(0, 8), 1, 25);
        Assert.True(database.Items.Count() == 0);
    }

    [Fact]
    public async Task AddAndUpdateDatabase()
    {
        var databaseName = $"[{StringUtilities.RandomString(10)}]";
        var databaseUser = $"[{StringUtilities.RandomString(10)}]";
        var databaseId = await _locator.AddDatabase(
            databaseName,
            databaseUser,
            _databaseServerID,
            _databaseTypeId,
            Status.Active,
            false,
            true
        );

        var databaseName2 = $"[{StringUtilities.RandomString(10)}]";
        var databaseUser2 = $"[{StringUtilities.RandomString(10)}]";
        await _locator.UpdateDatabase(
            databaseId,
            databaseName2,
            databaseUser2,
            _databaseServerID,
            _databaseTypeId,
            Status.Inactive
        );

        var oldDatabases = (
            await _locator.GetDatabases(databaseName.Substring(0, 8), 1, 25)
        ).Items.ToList();
        Assert.True(oldDatabases.Count() == 0);

        var newDatabases = (
            await _locator.GetDatabases(databaseName2.Substring(0, 8), 1, 25)
        ).Items.ToList();
        Assert.Equal(databaseName2, newDatabases[0].Name);
        Assert.Equal(Status.Inactive, newDatabases[0].Status);
    }
}

using System.Linq;
using Locator;
using LocatorTests.Fixtures;

namespace LocatorTests;

[Collection("Locator")]
public class DatabaseTypeTests
{
    private readonly Locator.Locator _locator;

    public DatabaseTypeTests(LocatorFixture locatorFixture)
    {
        _locator = locatorFixture.Locator;
    }

    [Fact]
    public async Task AddMultipleDatabaseTypesAndSearchByKeyWord()
    {
        var databaseTypeName = StringUtilities.RandomString(10);
        var databaseTypeId = await _locator.AddDatabaseType(databaseTypeName);

        var databaseTypeName2 = StringUtilities.RandomString(10);
        var databaseTypeId2 = await _locator.AddDatabaseType(databaseTypeName2);

        var databaseTypes = (await _locator.GetDatabaseTypes())
            .Where(x => x.Name == databaseTypeName)
            .ToList();

        Assert.True(databaseTypes.Count() == 1);
        Assert.Equal(databaseTypeName, databaseTypes[0].Name);
    }

    [Fact]
    public async Task AddAndDeleteDatabaseType()
    {
        var databaseTypeName = StringUtilities.RandomString(10);
        var databaseTypeId = await _locator.AddDatabaseType(databaseTypeName);

        await _locator.DeleteDatabaseType(databaseTypeId);
        var databaseType = (await _locator.GetDatabaseTypes())
            .Where(x => x.Name == databaseTypeName)
            .ToList();
        Assert.True(databaseType.Count() == 0);
    }

    [Fact]
    public async Task AddAndUpdateDatabaseType()
    {
        var databaseTypeName = StringUtilities.RandomString(10);
        var databaseTypeId = await _locator.AddDatabaseType(databaseTypeName);

        var databaseTypeName2 = StringUtilities.RandomString(10);
        await _locator.UpdateDatabaseType(databaseTypeId, databaseTypeName2);

        var oldDatabaseTypes = (await _locator.GetDatabaseTypes())
            .Where(x => x.Name == databaseTypeName)
            .ToList();
        Assert.True(oldDatabaseTypes.Count() == 0);

        var newDatabaseTypes = (await _locator.GetDatabaseTypes())
            .Where(x => x.Name == databaseTypeName2)
            .ToList();
        Assert.True(newDatabaseTypes.Count() == 1);
    }
}

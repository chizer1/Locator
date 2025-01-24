using LocatorTests.Fixtures;
using LocatorTests.Utilities;

namespace LocatorTests;

[Collection("Locator")]
public class DatabaseTypeTests(LocatorFixture locatorFixture)
{
    private readonly Locator.Locator _locator = locatorFixture.Locator;

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

        Assert.Single(databaseTypes);
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

        Assert.Empty(databaseType);
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
        Assert.Empty(oldDatabaseTypes);

        var newDatabaseTypes = (await _locator.GetDatabaseTypes())
            .Where(x => x.Name == databaseTypeName2)
            .ToList();
        Assert.Single(newDatabaseTypes);
    }
}

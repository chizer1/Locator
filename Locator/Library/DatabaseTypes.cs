using Locator.Domain;
using Locator.Features.DatabaseTypes;
using Locator.Features.DatabaseTypes.AddDatabaseType;
using Locator.Features.DatabaseTypes.DeleteDatabaseType;
using Locator.Features.DatabaseTypes.GetDatabaseTypes;
using Locator.Features.DatabaseTypes.UpdateDatabaseType;
using Microsoft.EntityFrameworkCore;

namespace Locator.Library;

internal class DatabaseTypes
{
    private readonly AddDatabaseType _addDatabaseType;
    private readonly GetDatabaseTypes _getDatabaseTypes;
    private readonly UpdateDatabaseType _updateDatabaseType;
    private readonly DeleteDatabaseType _deleteDatabaseType;

    public DatabaseTypes(DbContext locatorDb)
    {
        IDatabaseTypeRepository databaseTypeRepository = new DatabaseTypeRepository(locatorDb);

        _addDatabaseType = new AddDatabaseType(databaseTypeRepository);
        _getDatabaseTypes = new GetDatabaseTypes(databaseTypeRepository);
        _updateDatabaseType = new UpdateDatabaseType(databaseTypeRepository);
        _deleteDatabaseType = new DeleteDatabaseType(databaseTypeRepository);
    }

    public async Task<int> AddDatabaseType(string databaseTypeName)
    {
        return await _addDatabaseType.Handle(new AddDatabaseTypeCommand(databaseTypeName));
    }

    public async Task<List<DatabaseType>> GetDatabaseTypes()
    {
        return await _getDatabaseTypes.Handle(new GetDatabaseTypesQuery());
    }

    public async Task UpdateDatabaseType(int databaseTypeId, string databaseTypeName)
    {
        await _updateDatabaseType.Handle(
            new UpdateDatabaseTypeCommand(databaseTypeId, databaseTypeName)
        );
    }

    public async Task DeleteDatabaseType(int databaseTypeId)
    {
        await _deleteDatabaseType.Handle(new DeleteDatabaseTypeCommand(databaseTypeId));
    }
}

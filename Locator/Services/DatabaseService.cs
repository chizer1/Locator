using Locator.Models;
using Locator.Repositories;

namespace Locator.Services;

internal class DatabaseService(DatabaseRepository databaseRepository)
{
    public async Task<int> AddDatabaseServer(string databaseServerName, string ipAddress)
    {
        return await databaseRepository.AddDatabaseServer(databaseServerName, ipAddress);
    }

    public async Task<int> AddDatabase(
        string databaseName,
        string databaseUser,
        string databaseUserPassword,
        int databaseServerId,
        int databaseTypeId,
        DatabaseStatus databaseStatus
    )
    {
        return await databaseRepository.AddDatabase(
            databaseName,
            databaseUser,
            databaseUserPassword,
            databaseServerId,
            databaseTypeId,
            databaseStatus
        );
    }

    public async Task<int> AddDatabaseType(string databaseTypeName)
    {
        return await databaseRepository.AddDatabaseType(databaseTypeName);
    }

    public async Task<List<DatabaseType>> GetDatabaseTypes()
    {
        return await databaseRepository.GetDatabaseTypes();
    }

    // public async Task<DatabaseType> GetDatabaseType(int databaseTypeId)
    // {
    //     return await databaseRepository.GetDatabaseType(databaseTypeId);
    // }

    public async Task UpdateDatabaseType(int databaseTypeId, string databaseTypeName)
    {
        await databaseRepository.UpdateDatabaseType(databaseTypeId, databaseTypeName);
    }

    // public async Task DeleteDatabaseType(int databaseTypeId)
    // {
    //     await databaseRepository.DeleteDatabaseType(databaseTypeId);
    // }

    public async Task<List<DatabaseServer>> GetDatabaseServers()
    {
        return await databaseRepository.GetDatabaseServers();
    }

    // get databases
    public async Task<List<Database>> GetDatabases()
    {
        return await databaseRepository.GetDatabases();
    }
}

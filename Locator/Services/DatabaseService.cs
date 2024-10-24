using Locator.Models;
using Locator.Repositories;

namespace Locator.Services;

internal class DatabaseService(DatabaseRepository databaseRepository)
{
    // copy all methods from repo

    public async Task<int> AddDatabaseServer(
        string databaseServerName,
        string ipAddress,
        int userId
    )
    {
        return await databaseRepository.AddDatabaseServer(databaseServerName, ipAddress, userId);
    }

    public async Task<int> AddDatabase(
        string databaseName,
        string databaseUser,
        string databaseUserPassword,
        int databaseServerId,
        int databaseTypeId,
        int userId
    )
    {
        return await databaseRepository.AddDatabase(
            databaseName,
            databaseUser,
            databaseUserPassword,
            databaseServerId,
            databaseTypeId,
            userId
        );
    }

    public async Task<int> AddDatabaseType(string databaseTypeName, int userId)
    {
        return await databaseRepository.AddDatabaseType(databaseTypeName, userId);
    }

    public async Task<List<DatabaseType>> GetDatabaseTypes()
    {
        return await databaseRepository.GetDatabaseTypes();
    }

    // public async Task<DatabaseType> GetDatabaseType(int databaseTypeId)
    // {
    //     return await databaseRepository.GetDatabaseType(databaseTypeId);
    // }

    public async Task UpdateDatabaseType(
        int databaseTypeId,
        string databaseTypeName,
        int modifyById
    )
    {
        await databaseRepository.UpdateDatabaseType(databaseTypeId, databaseTypeName, modifyById);
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

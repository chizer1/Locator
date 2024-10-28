using Locator.Models;
using Locator.Repositories;

namespace Locator.Services;

internal class DatabaseService(DatabaseRepository databaseRepository)
{
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

    public async Task<List<Database>> GetDatabases()
    {
        return await databaseRepository.GetDatabases();
    }

    public async Task DeleteDatabase(Database database)
    {
        await databaseRepository.DeleteDatabase(database);
    }

    public async Task<Database> GetDatabase(int databaseId)
    {
        return await databaseRepository.GetDatabase(databaseId);
    }
}

using Locator.Common.Models;
using Locator.Domain;

namespace Locator.Features.Databases;

internal interface IDatabaseRepository
{
    public Task<int> AddDatabase(
        string databaseName,
        string databaseUser,
        int databaseServerId,
        byte databaseTypeId,
        Status databaseStatus
    );
    public Task<Database> GetDatabase(int databaseId);
    public Task<PagedList<Database>> GetDatabases(string keyword, int pageNumber, int pageSize);
    public Task UpdateDatabase(
        int databaseId,
        string databaseName,
        string databaseUserName,
        int databaseServerId,
        byte databaseTypeId,
        Status databaseStatus
    );
    public Task DeleteDatabase(int databaseId);
}

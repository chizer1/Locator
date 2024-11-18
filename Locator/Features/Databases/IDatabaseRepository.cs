using Locator.Common.Models;
using Locator.Domain;

namespace Locator.Features.Databases;

public interface IDatabaseRepository
{
    public Task<int> AddDatabase(
        string databaseName,
        string databaseUser,
        int databaseServerId,
        int databaseTypeId,
        DatabaseStatus databaseStatus
    );

    public Task<List<Database>> GetDatabases();

    public Task<PagedList<Database>> GetDatabases(string keyword, int pageNumber, int pageSize);

    public Task UpdateDatabase(
        int databaseId,
        string databaseName,
        string databaseUserName,
        int databaseServerId,
        int databaseTypeId,
        DatabaseStatus databaseStatus
    );

    public Task DeleteDatabase(int databaseId);
}

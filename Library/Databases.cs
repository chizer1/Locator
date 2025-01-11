using Locator.Common.Models;
using Locator.Domain;
using Locator.Features.Databases;
using Locator.Features.Databases.AddDatabase;
using Locator.Features.Databases.DeleteDatabase;
using Locator.Features.Databases.GetDatabases;
using Locator.Features.Databases.UpdateDatabase;
using Microsoft.EntityFrameworkCore;

namespace Locator.Library;

internal class Databases
{
    private readonly AddDatabase _addDatabase;
    private readonly GetDatabases _getDatabases;
    private readonly UpdateDatabase _updateDatabase;
    private readonly DeleteDatabase _deleteDatabase;

    public Databases(DbContext locatorDb)
    {
        IDatabaseRepository databaseRepository = new DatabaseRepository(locatorDb);

        _addDatabase = new AddDatabase(databaseRepository);
        _getDatabases = new GetDatabases(databaseRepository);
        _updateDatabase = new UpdateDatabase(databaseRepository);
        _deleteDatabase = new DeleteDatabase(databaseRepository);
    }

    public async Task<int> AddDatabase(
        string databaseName,
        string databaseUser,
        int databaseServerId,
        byte databaseTypeId,
        Status databaseStatus,
        bool useTrustedConnection,
        bool createDatabase
    )
    {
        return await _addDatabase.Handle(
            new AddDatabaseCommand(
                databaseName,
                databaseUser,
                databaseServerId,
                databaseTypeId,
                databaseStatus,
                useTrustedConnection,
                createDatabase
            )
        );
    }

    public async Task<PagedList<Database>> GetDatabases(
        string keyword,
        int pageNumber,
        int pageSize
    )
    {
        return await _getDatabases.Handle(new GetDatabasesQuery(keyword, pageNumber, pageSize));
    }

    public async Task UpdateDatabase(
        int databaseId,
        string databaseName,
        string databaseUser,
        int databaseServerId,
        byte databaseTypeId,
        Status databaseStatus
    )
    {
        await _updateDatabase.Handle(
            new UpdateDatabaseCommand(
                databaseId,
                databaseName,
                databaseUser,
                databaseServerId,
                databaseTypeId,
                databaseStatus
            )
        );
    }

    public async Task DeleteDatabase(int databaseId)
    {
        await _deleteDatabase.Handle(new DeleteDatabaseCommand(databaseId));
    }
}

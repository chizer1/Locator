using Locator.Db;
using Locator.Domain;
using Microsoft.EntityFrameworkCore;

namespace Locator.Features.DatabaseTypes;

internal class DatabaseTypeRepository(DbContext locatorDb) : IDatabaseTypeRepository
{
    public async Task<int> AddDatabaseType(string databaseTypeName)
    {
        var databaseType = new DatabaseTypeEntity { DatabaseTypeName = databaseTypeName };
        locatorDb.Add(databaseType);
        await locatorDb.SaveChangesAsync();
        return databaseType.DatabaseTypeId;
    }

    public async Task<DatabaseType> GetDatabaseType(int databaseTypeId)
    {
        return await locatorDb
            .Set<DatabaseType>()
            .FirstOrDefaultAsync(dt => dt.Id == databaseTypeId);
    }

    public async Task<List<DatabaseType>> GetDatabaseTypes()
    {
        var databaseTypeEntities = await locatorDb.Set<DatabaseTypeEntity>().ToListAsync();

        return databaseTypeEntities
            .Select(entity => new DatabaseType(entity.DatabaseTypeId, entity.DatabaseTypeName))
            .ToList();
    }

    public async Task UpdateDatabaseType(int databaseTypeId, string databaseTypeName)
    {
        var databaseType = await locatorDb
            .Set<DatabaseTypeEntity>()
            .FirstOrDefaultAsync(dt => dt.DatabaseTypeId == databaseTypeId);

        if (databaseType != null)
        {
            databaseType.DatabaseTypeName = databaseTypeName;
            locatorDb.Update(databaseType);
            await locatorDb.SaveChangesAsync();
        }
    }

    public async Task DeleteDatabaseType(int databaseTypeId)
    {
        var databaseType = await locatorDb
            .Set<DatabaseTypeEntity>()
            .FirstOrDefaultAsync(dt => dt.DatabaseTypeId == databaseTypeId);

        if (databaseType != null)
        {
            locatorDb.Remove(databaseType);
            await locatorDb.SaveChangesAsync();
        }
    }
}

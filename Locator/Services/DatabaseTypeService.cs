using System.ComponentModel.DataAnnotations;
using Locator.Models;
using Locator.Repositories;

namespace Locator.Services;

internal class DatabaseTypeService(DatabaseTypeRepository databaseTypeRepository)
{
    public async Task<int> AddDatabaseType([Required] string databaseTypeName)
    {
        return await databaseTypeRepository.AddDatabaseType(databaseTypeName);
    }

    public async Task<List<DatabaseType>> GetDatabaseTypes()
    {
        return await databaseTypeRepository.GetDatabaseTypes();
    }

    public async Task UpdateDatabaseType(
        [Required] int databaseTypeId,
        [Required] string databaseTypeName
    )
    {
        await databaseTypeRepository.UpdateDatabaseType(databaseTypeId, databaseTypeName);
    }

    public async Task DeleteDatabaseType([Required] int databaseTypeId)
    {
        await databaseTypeRepository.DeleteDatabaseType(databaseTypeId);
    }
}

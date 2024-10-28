using System.ComponentModel.DataAnnotations;
using Locator.Models;
using Locator.Repositories;

namespace Locator.Services;

internal class DatabaseServerService(DatabaseServerRepository databaseServerRepository)
{
    public async Task<int> AddDatabaseServer(
        [Required] string serverName,
        [Required] string serverIpAddress
    )
    {
        return await databaseServerRepository.AddDatabaseServer(serverName, serverIpAddress);
    }

    public async Task<List<DatabaseServer>> GetDatabaseServers()
    {
        return await databaseServerRepository.GetDatabaseServers();
    }

    public async Task<DatabaseServer> GetDatabaseServer([Required] int databaseServerId)
    {
        return await databaseServerRepository.GetDatabaseServer(databaseServerId);
    }

    public async Task UpdateDatabaseServer(
        [Required] int databaseServerId,
        [Required] string databaseServerName,
        [Required] string databaseServerIpAddress
    )
    {
        await databaseServerRepository.UpdateDatabaseServer(
            databaseServerId,
            databaseServerName,
            databaseServerIpAddress
        );
    }

    public async Task DeleteDatabaseServer([Required] int databaseServerId)
    {
        await databaseServerRepository.DeleteDatabaseServer(databaseServerId);
    }
}

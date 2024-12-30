using Locator.Db;
using Locator.Domain;
using Microsoft.EntityFrameworkCore;

namespace Locator.Features.DatabaseServers
{
    internal class DatabaseServerRepository(DbContext locatorDb) : IDatabaseServerRepository
    {
        public async Task<int> AddDatabaseServer(
            string databaseServerName,
            string databaseServerIpAddress
        )
        {
            var databaseServer = new DatabaseServerEntity
            {
                DatabaseServerName = databaseServerName,
                DatabaseServerIpaddress = databaseServerIpAddress,
            };

            locatorDb.Add(databaseServer);
            await locatorDb.SaveChangesAsync();

            return databaseServer.DatabaseServerId;
        }

        public async Task<DatabaseServer> GetDatabaseServer(int databaseServerId)
        {
            var databaseServerEntity = await locatorDb
                .Set<DatabaseServerEntity>()
                .Where(ds => ds.DatabaseServerId == databaseServerId)
                .FirstOrDefaultAsync();

            return new DatabaseServer(
                databaseServerEntity.DatabaseServerId,
                databaseServerEntity.DatabaseServerName,
                databaseServerEntity.DatabaseServerIpaddress
            );
        }

        public async Task<List<DatabaseServer>> GetDatabaseServers()
        {
            var databaseServerEntities = await locatorDb.Set<DatabaseServerEntity>().ToListAsync();

            return databaseServerEntities
                .Select(ds => new DatabaseServer(
                    ds.DatabaseServerId,
                    ds.DatabaseServerName,
                    ds.DatabaseServerIpaddress
                ))
                .ToList();
        }

        public async Task UpdateDatabaseServer(
            int databaseServerId,
            string databaseServerName,
            string databaseServerIpAddress
        )
        {
            var databaseServer =
                await locatorDb
                    .Set<DatabaseServerEntity>()
                    .FirstOrDefaultAsync(ds => ds.DatabaseServerId == databaseServerId)
                ?? throw new KeyNotFoundException(
                    $"Database Server with ID {databaseServerId} not found."
                );

            databaseServer.DatabaseServerName = databaseServerName;
            databaseServer.DatabaseServerIpaddress = databaseServerIpAddress;

            locatorDb.Update(databaseServer);
            await locatorDb.SaveChangesAsync();
        }

        public async Task DeleteDatabaseServer(int databaseServerId)
        {
            var databaseServer =
                await locatorDb
                    .Set<DatabaseServerEntity>()
                    .FirstOrDefaultAsync(ds => ds.DatabaseServerId == databaseServerId)
                ?? throw new KeyNotFoundException(
                    $"Database Server with ID {databaseServerId} not found."
                );

            locatorDb.Remove(databaseServer);
            await locatorDb.SaveChangesAsync();
        }
    }
}

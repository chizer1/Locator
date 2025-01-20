using Locator.Common.Models;
using Locator.Db;
using Locator.Domain;
using Microsoft.EntityFrameworkCore;

namespace Locator.Features.Databases
{
    internal class DatabaseRepository(DbContext locatorDb) : IDatabaseRepository
    {
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
            var database = new DatabaseEntity
            {
                DatabaseName = databaseName,
                DatabaseServerId = databaseServerId,
                DatabaseTypeId = databaseTypeId,
                DatabaseStatusId = (byte)databaseStatus,
                UseTrustedConnection = useTrustedConnection,
            };

            var commands = new List<string>();

            if (createDatabase)
                commands.Add($"CREATE DATABASE {databaseName}");

            if (!useTrustedConnection)
            {
                database.DatabaseUser = databaseUser;

                var password = Guid.NewGuid().ToString();
                database.DatabaseUserPassword = password; // encrypt here later

                commands.AddRange(
                    [
                        $"CREATE LOGIN {databaseUser} WITH PASSWORD = '{password}'",
                        $"USE {databaseName}; CREATE USER {databaseUser} FOR LOGIN {databaseUser}",
                    ]
                );
            }

            await EnsureDatabaseExists(databaseName, createDatabase);
            await EnsureUserDoesNotExist(databaseUser);

            await locatorDb.Set<DatabaseEntity>().AddAsync(database);
            await locatorDb.SaveChangesAsync();

            foreach (var commandText in commands)
            {
                using var command = locatorDb.Database.GetDbConnection().CreateCommand();
                command.CommandText = commandText;
                await locatorDb.Database.OpenConnectionAsync();
                await command.ExecuteNonQueryAsync();
            }

            await SwapBackToLocatorDatabase();

            return database.DatabaseId;
        }

        private async Task EnsureDatabaseExists(string databaseName, bool createDatabase)
        {
            using var command = locatorDb.Database.GetDbConnection().CreateCommand();
            command.CommandText =
                $"SELECT COUNT(*) FROM sys.databases WHERE name = '{databaseName}'";
            await locatorDb.Database.OpenConnectionAsync();
            var result = await command.ExecuteScalarAsync();
            if (result is int count)
            {
                if (createDatabase && count > 0)
                    throw new InvalidOperationException($"Database {databaseName} already exists.");
                if (!createDatabase && count == 0)
                    throw new InvalidOperationException($"Database {databaseName} not found.");
            }
        }

        private async Task EnsureUserDoesNotExist(string databaseUser)
        {
            using var command = locatorDb.Database.GetDbConnection().CreateCommand();
            command.CommandText =
                $"SELECT COUNT(*) FROM sys.server_principals WHERE name = '{databaseUser}'";
            await locatorDb.Database.OpenConnectionAsync();
            var result = await command.ExecuteScalarAsync();
            if (result is int count && count > 0)
                throw new InvalidOperationException($"User {databaseUser} already exists.");
        }

        public async Task<Database> GetDatabase(int databaseId)
        {
            var database =
                await locatorDb
                    .Set<DatabaseEntity>()
                    .FirstOrDefaultAsync(d => d.DatabaseId == databaseId)
                ?? throw new InvalidOperationException("Database not found.");

            return new Database(
                database.DatabaseId,
                database.DatabaseName,
                database.DatabaseServerId,
                (Status)database.DatabaseStatusId
            );
        }

        public async Task<PagedList<Database>> GetDatabases(
            string keyword,
            int pageNumber,
            int pageSize
        )
        {
            var query = locatorDb.Set<DatabaseEntity>().AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(d => d.DatabaseName.Contains(keyword));

            var totalCount = await query.CountAsync();
            var databaseEntities = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var databases = databaseEntities
                .Select(d => new Database(
                    d.DatabaseId,
                    d.DatabaseName,
                    d.DatabaseServerId,
                    (Status)d.DatabaseStatusId
                ))
                .ToList();

            return new PagedList<Database>(
                databases,
                totalCount,
                pageNumber,
                pageSize,
                (int)Math.Ceiling((double)totalCount / pageSize)
            );
        }

        public async Task UpdateDatabase(
            int databaseId,
            string databaseName,
            string databaseUserName,
            int databaseServerId,
            byte databaseTypeId,
            Status databaseStatus
        )
        {
            var databaseEntity =
                await locatorDb
                    .Set<DatabaseEntity>()
                    .FirstOrDefaultAsync(d => d.DatabaseId == databaseId)
                ?? throw new InvalidOperationException("Database not found.");

            var oldDatabaseName = databaseEntity.DatabaseName;
            var oldDatabaseUser = databaseEntity.DatabaseUser;

            databaseEntity.DatabaseName = databaseName;
            databaseEntity.DatabaseUser = databaseUserName;
            databaseEntity.DatabaseServerId = databaseServerId;
            databaseEntity.DatabaseTypeId = databaseTypeId;
            databaseEntity.DatabaseStatusId = (byte)databaseStatus;

            locatorDb.Update(databaseEntity);
            await locatorDb.SaveChangesAsync();

            var commands = new List<string>();
            if (oldDatabaseName != databaseName)
                commands.Add($"ALTER DATABASE {oldDatabaseName} MODIFY NAME = {databaseName}");

            if (oldDatabaseUser != databaseUserName)
                commands.Add(
                    $"USE {databaseName}; ALTER USER {oldDatabaseUser} WITH NAME = {databaseUserName}"
                );

            foreach (var commandText in commands)
            {
                using var command = locatorDb.Database.GetDbConnection().CreateCommand();
                command.CommandText = commandText;
                await locatorDb.Database.OpenConnectionAsync();
                await command.ExecuteNonQueryAsync();
            }

            await SwapBackToLocatorDatabase();
        }

        public async Task DeleteDatabase(int databaseId)
        {
            var databaseEntity =
                await locatorDb.Set<DatabaseEntity>().FindAsync(databaseId)
                ?? throw new InvalidOperationException("Database not found.");

            locatorDb.Set<DatabaseEntity>().Remove(databaseEntity);
            await locatorDb.SaveChangesAsync();

            var commands = new[]
            {
                $"DROP DATABASE {databaseEntity.DatabaseName}",
                $"DROP LOGIN {databaseEntity.DatabaseUser}",
            };

            foreach (var commandText in commands)
            {
                using var command = locatorDb.Database.GetDbConnection().CreateCommand();
                command.CommandText = commandText;
                await locatorDb.Database.OpenConnectionAsync();
                await command.ExecuteNonQueryAsync();
            }

            await SwapBackToLocatorDatabase();
        }

        private async Task SwapBackToLocatorDatabase()
        {
            using var swapCommand = locatorDb.Database.GetDbConnection().CreateCommand();
            swapCommand.CommandText = "use [Locator]";
            await locatorDb.Database.OpenConnectionAsync();
            await swapCommand.ExecuteNonQueryAsync();
        }
    }
}

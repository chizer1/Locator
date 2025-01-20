using Locator.Db;
using Microsoft.EntityFrameworkCore;

namespace Locator.Features.ClientUsers;

internal class ClientUserRepository(DbContext locatorDb) : IClientUserRepository
{
    public async Task<int> AddClientUser(int clientId, int userId)
    {
        var clientUser = new ClientUserEntity { ClientId = clientId, UserId = userId };

        await locatorDb.Set<ClientUserEntity>().AddAsync(clientUser);
        await locatorDb.SaveChangesAsync();
        return clientUser.ClientUserId;
    }

    public async Task DeleteClientUser(int clientId, int userId)
    {
        var clientUser =
            await locatorDb
                .Set<ClientUserEntity>()
                .FirstOrDefaultAsync(cu => cu.ClientId == clientId && cu.UserId == userId)
            ?? throw new KeyNotFoundException(
                $"ClientUser with ClientId {clientId} and UserId {userId} not found."
            );
        locatorDb.Set<ClientUserEntity>().Remove(clientUser);
        await locatorDb.SaveChangesAsync();
    }
}

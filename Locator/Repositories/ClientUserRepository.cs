using System.Data;
using Dapper;

namespace Locator.Repositories;

internal class ClientUserRepository(IDbConnection locatorDb)
{
    public async Task<int> AddClientUser(int clientId, int userId)
    {
        return await locatorDb.QuerySingleAsync<int>(
            @$"
            insert into dbo.ClientUser
            (
                ClientID,
                UserID
            )
            values
            (
                @ClientID,
                @UserID
            )

            select scope_identity()",
            new { clientId, userId }
        );
    }

    public async Task DeleteClientUser(int clientUserId)
    {
        await locatorDb.ExecuteAsync(
            @$"
            delete from dbo.ClientUser
            where ClientUserID = @ClientUserID",
            new { clientUserId }
        );
    }
}

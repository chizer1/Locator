using System.Data;
using Dapper;
using Locator.Models;

namespace Locator.Repositories;

internal class ClientRepository(IDbConnection locatorDb)
{
    public async Task<int> AddClient(
        string clientName,
        string clientCode,
        ClientStatus clientStatus
    )
    {
        return await locatorDb.QuerySingleAsync<int>(
            @$"
            insert into dbo.Client 
            (
                ClientCode,
                ClientName, 
                ClientStatusID
            )
            values 
            (
                @ClientCode,
                @ClientName,
                @ClientStatusID,
            )

            select scope_identity()",
            new
            {
                clientCode,
                clientName,
                ClientStatusID = (int)clientStatus,
            }
        );
    }

    public async Task<Client> GetClient(int clientId)
    {
        return await locatorDb.QuerySingleAsync<Client>(
            @$"
            select
                ClientID {nameof(Client.ClientId)},
                ClientCode {nameof(Client.ClientCode)},
                ClientName {nameof(Client.ClientName)},
                ClientStatusID {nameof(Client.ClientStatusId)}
            from dbo.Client 
            where
                ClientID = @ClientID",
            new { clientId }
        );
    }

    public async Task<List<Client>> GetClients()
    {
        return (
            await locatorDb.QueryAsync<Client>(
                @$"
                select
                    ClientID {nameof(Client.ClientId)},
                    ClientCode {nameof(Client.ClientCode)},
                    ClientName {nameof(Client.ClientName)},
                    ClientStatusID {nameof(Client.ClientStatusId)}
                from dbo.Client
                order by
                    ClientName asc"
            )
        ).ToList();
    }

    public async Task<PagedList<Client>> GetClients(string keyword, int pageNumber, int pageSize)
    {
        var whereClause = keyword == null ? "" : "where c.ClientName like '%' + @Keyword + '%'";
        var results = await locatorDb.QueryMultipleAsync(
            @$"
            select count(*) as TotalCount from dbo.Client c {whereClause}

            select
                c.ClientID {nameof(Client.ClientId)},
                c.ClientCode {nameof(Client.ClientCode)},
                c.ClientName {nameof(Client.ClientName)},
                c.ClientStatusID {nameof(Client.ClientStatusId)}
            from dbo.Client c
            inner join dbo.ClientStatus cs
                on c.ClientStatusID = cs.ClientStatusID
            {whereClause}
            order by
                c.ClientName asc
            offset ((@PageNumber - 1) * @PageSize) rows
            fetch next @PageSize rows only",
            new
            {
                keyword,
                pageNumber,
                pageSize,
            }
        );

        int rowCount = results.Read<int>().First();
        return new(
            results.Read<Client>().ToList(),
            rowCount,
            pageNumber,
            pageSize,
            (int)Math.Ceiling((double)rowCount / pageSize)
        );
    }

    public async Task UpdateClient(
        int clientId,
        string clientName,
        string clientCode,
        ClientStatus clientStatus
    )
    {
        await locatorDb.ExecuteAsync(
            @$"
            update [Client]
            set
                ClientName = @ClientName,
                ClientCode = @ClientCode,
                ClientStatusID = @ClientStatusID
            where
                ClientID = @ClientID",
            new
            {
                clientId,
                clientName,
                clientCode,
                ClientStatusID = (int)clientStatus,
            }
        );
    }

    public async Task DeleteClient(int clientId)
    {
        await locatorDb.ExecuteAsync(
            "delete from dbo.Client where ClientID = @ClientID",
            new { clientId }
        );
    }
}

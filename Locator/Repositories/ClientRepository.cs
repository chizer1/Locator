using System.Data;
using Dapper;
using Locator.Domain;
using Locator.Models.Read;
using Locator.Models.Write;

namespace Locator.Repositories;

internal class ClientRepository(IDbConnection locatorDb)
{
    public async Task<int> AddClient(AddClient addClient)
    {
        return await locatorDb.QuerySingleAsync<int>(
            @$"
            insert into dbo.Client 
            (
                ClientName, 
                ClientCode,
                ClientStatusID
            )
            values 
            (
                @ClientName,
                @ClientCode,
                @ClientStatusID
            )

            select scope_identity()",
            new
            {
                addClient.ClientCode,
                addClient.ClientName,
                ClientStatusID = (int)addClient.ClientStatus,
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
                ClientStatusID {nameof(Client.ClientStatus)}
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
                    ClientStatusID {nameof(Client.ClientStatus)}
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
                c.ClientStatusID {nameof(Client.ClientStatus)}
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

        var rowCount = results.Read<int>().First();
        return new PagedList<Client>(
            results.Read<Client>().ToList(),
            rowCount,
            pageNumber,
            pageSize,
            (int)Math.Ceiling((double)rowCount / pageSize)
        );
    }

    public async Task UpdateClient(UpdateClient updateClient)
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
                updateClient.ClientId,
                updateClient.ClientCode,
                updateClient.ClientName,
                ClientStatusID = (int)updateClient.ClientStatus,
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

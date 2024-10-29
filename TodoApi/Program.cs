using System.Data.SqlClient;
using Locator;
using Locator.Models.Read;
using Locator.Models.Write;

LocatorLib locator =
    new(
        new SqlConnection("Server=localhost;Database=Locator;User Id=sa;Password=1StrongPwd!!;"),
        "https://dev-xshhwrh4f1vis6lb.us.auth0.com/",
        "RCbDL6LnErLJfuXz1s3hvLf6bVJklNFl",
        "auY2mDwrU8dPwlU8cMI4iLd2q4Lsu5nvhsF60DCRWk542AadNTQ8lV_jHp0MzRAF"
    );

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

#region User Endpoints

app.MapPost(
    "/addUser",
    async (
        string firstName,
        string lastName,
        string emailAddress,
        List<Role> roles,
        UserStatus userStatus
    ) =>
    {
        return await locator.AddUser(firstName, lastName, emailAddress, roles, userStatus);
    }
);

app.MapGet(
    "/getUser",
    async (int userId) =>
    {
        return await locator.GetUser(userId);
    }
);

app.MapGet(
    "/getUsers",
    async () =>
    {
        return await locator.GetUsers();
    }
);

app.MapPut(
    "/updateUser",
    async (
        int userId,
        string auth0Id,
        string firstName,
        string lastName,
        string emailAddress,
        UserStatus userStatus,
        List<Role> roles
    ) =>
    {
        await locator.UpdateUser(
            userId,
            auth0Id,
            firstName,
            lastName,
            emailAddress,
            userStatus,
            roles
        );
    }
);

app.MapDelete(
    "/deleteUser",
    async (string auth0Id) =>
    {
        await locator.DeleteUser(auth0Id);
    }
);

#endregion

#region Client Endpoints

app.MapPost(
    "/addClient",
    async (string clientName, string clientCode, ClientStatus clientStatus) =>
    {
        return await locator.AddClient(clientName, clientCode, clientStatus);
    }
);

app.MapGet(
    "/getClient",
    async (int clientId) =>
    {
        return await locator.GetClient(clientId);
    }
);

app.MapGet(
    "/getClients",
    async () =>
    {
        return await locator.GetClients();
    }
);

app.MapPut(
    "/updateClient",
    async (int clientId, string clientName, string clientCode, ClientStatus clientStatus) =>
    {
        await locator.UpdateClient(clientId, clientName, clientCode, clientStatus);
    }
);

app.MapDelete(
    "/deleteClient",
    async (int clientId) =>
    {
        await locator.DeleteClient(clientId);
    }
);

#endregion

#region Database Server Endpoints

app.MapPost(
    "/addDatabaseServer",
    async (string serverName, string serverIpAddress) =>
    {
        return await locator.AddDatabaseServer(serverName, serverIpAddress);
    }
);

app.MapGet(
    "/getDatabaseServer",
    async (int serverId) =>
    {
        return await locator.GetDatabaseServer(serverId);
    }
);

app.MapGet(
    "/getDatabaseServers",
    async () =>
    {
        return await locator.GetDatabaseServers();
    }
);

app.MapPut(
    "/updateDatabaseServer",
    async (int serverId, string serverName, string serverIpAddress) =>
    {
        await locator.UpdateDatabaseServer(serverId, serverName, serverIpAddress);
    }
);

app.MapDelete(
    "/deleteDatabaseServer",
    async (int serverId) =>
    {
        await locator.DeleteDatabaseServer(serverId);
    }
);

#endregion

#region Database Endpoints

app.MapPost(
    "/addDatabase",
    async (AddDatabase addDatabase) =>
    {
        return await locator.AddDatabase(addDatabase);
    }
);

app.MapGet(
    "/getDatabase",
    async (int databaseId) =>
    {
        return await locator.GetDatabase(databaseId);
    }
);

app.MapGet(
    "/getDatabases",
    async () =>
    {
        return await locator.GetDatabases();
    }
);

app.MapPut(
    "/updateDatabase",
    async (UpdateDatabase updateDatabase) =>
    {
        await locator.UpdateDatabase(updateDatabase);
    }
);

app.MapDelete(
    "/deleteDatabase",
    async (int databaseId) =>
    {
        await locator.DeleteDatabase(databaseId);
    }
);

#endregion

app.Run();

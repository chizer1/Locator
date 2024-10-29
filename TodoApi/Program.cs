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
    async (AddUser addUser) =>
    {
        return await locator.AddUser(addUser);
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
    async (UpdateUser updateUser) =>
    {
        await locator.UpdateUser(updateUser);
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
    async (AddClient addClient) =>
    {
        return await locator.AddClient(addClient);
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
    async (UpdateClient updateClient) =>
    {
        await locator.UpdateClient(updateClient);
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

#region Role Endpoints

app.MapPost(
    "/addRole",
    async (AddRole addRole) =>
    {
        return await locator.AddRole(addRole);
    }
);

app.MapGet(
    "/getRole",
    async (int roleId) =>
    {
        return await locator.GetRole(roleId);
    }
);

app.MapGet(
    "/getRoles",
    async () =>
    {
        return await locator.GetRoles();
    }
);

app.MapPut(
    "/updateRole",
    async (UpdateRole updateRole) =>
    {
        await locator.UpdateRole(updateRole);
    }
);

app.MapDelete(
    "/deleteRole",
    async (int roleId) =>
    {
        await locator.DeleteRole(roleId);
    }
);

#endregion

#region User Role Endpoints

app.MapPost(
    "/addUserRole",
    async (int userId, int roleId) =>
    {
        return await locator.AddUserRole(userId, roleId);
    }
);

app.MapDelete(
    "/deleteUserRole",
    async (int userId, int roleId) =>
    {
        await locator.DeleteUserRole(userId, roleId);
    }
);

#endregion

#region Client User Endpoints

app.MapPost(
    "/addClientUser",
    async (int clientId, int userId) =>
    {
        return await locator.AddClientUser(clientId, userId);
    }
);

app.MapDelete(
    "/deleteClientUser",
    async (int clientId, int userId) =>
    {
        await locator.DeleteClientUser(clientId, userId);
    }
);

#endregion

#region Database Server Endpoints

app.MapPost(
    "/addDatabaseServer",
    async (AddDatabaseServer addDatabaseServer) =>
    {
        return await locator.AddDatabaseServer(addDatabaseServer);
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
    async (UpdateDatabaseServer updateDatabaseServer) =>
    {
        await locator.UpdateDatabaseServer(updateDatabaseServer);
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

#region Database Type Endpoints

app.MapPost(
    "/addDatabaseType",
    async (string name) =>
    {
        return await locator.AddDatabaseType(name);
    }
);

app.MapGet(
    "/getDatabaseType",
    async (int databaseTypeId) =>
    {
        return await locator.GetDatabaseType(databaseTypeId);
    }
);

app.MapGet(
    "/getDatabaseTypes",
    async () =>
    {
        return await locator.GetDatabaseTypes();
    }
);

app.MapPut(
    "/updateDatabaseType",
    async (int databaseTypeId, string name) =>
    {
        await locator.UpdateDatabaseType(databaseTypeId, name);
    }
);

app.MapDelete(
    "/deleteDatabaseType",
    async (int databaseTypeId) =>
    {
        await locator.DeleteDatabaseType(databaseTypeId);
    }
);

#endregion

#region Connection Endpoints

app.MapPost(
    "/addConnection",
    async (int clientId, int databaseId) =>
    {
        return await locator.AddConnection(clientId, databaseId);
    }
);

app.MapDelete(
    "/deleteConnection",
    async (int clientId, int databaseId) =>
    {
        await locator.DeleteConnection(clientId, databaseId);
    }
);

#endregion

app.Run();

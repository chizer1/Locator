using System.Data.SqlClient;
using Locator;
using Locator.Models.Write;

LocatorLib locator =
    new(
        new SqlConnection("Server=localhost;Database=Locator;User Id=sa;Password=1StrongPwd!!;"),
        "https://dev-xshhwrh4f1vis6lb.us.auth0.com/",
        "RCbDL6LnErLJfuXz1s3hvLf6bVJklNFl",
        "auY2mDwrU8dPwlU8cMI4iLd2q4Lsu5nvhsF60DCRWk542AadNTQ8lV_jHp0MzRAF"
    );

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorizationBuilder()
  .AddPolicy("admin_greetings", policy =>
        policy
            .RequireRole("admin")
            .RequireClaim("scope", "greetings_api"));

var app = builder.Build();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#region User Endpoints

app.MapPost(
    "/addUser",
    async (AddUser addUser) =>
    {
        return await locator.AddUser(addUser);
    }
).WithTags("User");

app.MapGet(
    "/getUser",
    async (int userId) =>
    {
        return await locator.GetUser(userId);
    }
).WithTags("User");

app.MapGet(
    "/getUsers",
    async () =>
    {
        return await locator.GetUsers();
    }
).WithTags("User");

app.MapPut(
    "/updateUser",
    async (UpdateUser updateUser) =>
    {
        await locator.UpdateUser(updateUser);
    }
).WithTags("User");

app.MapDelete(
    "/deleteUser",
    async (string auth0Id) =>
    {
        await locator.DeleteUser(auth0Id);
    }
).WithTags("User");

#endregion

#region Client Endpoints

app.MapPost(
    "/addClient",
    async (AddClient addClient) =>
    {
        return await locator.AddClient(addClient);
    }
).WithTags("Client");

app.MapGet(
    "/getClient",
    async (int clientId) =>
    {
        return await locator.GetClient(clientId);
    }
).WithTags("Client");

app.MapGet(
    "/getClients",
    async () =>
    {
        return await locator.GetClients();
    }
).WithTags("Client");

app.MapPut(
    "/updateClient",
    async (UpdateClient updateClient) =>
    {
        await locator.UpdateClient(updateClient);
    }
).WithTags("Client");

app.MapDelete(
    "/deleteClient",
    async (int clientId) =>
    {
        await locator.DeleteClient(clientId);
    }
).WithTags("Client");

#endregion

#region Role Endpoints

app.MapPost(
    "/addRole",
    async (AddRole addRole) =>
    {
        return await locator.AddRole(addRole);
    }
).WithTags("Role");

app.MapGet(
    "/getRole",
    async (int roleId) =>
    {
        return await locator.GetRole(roleId);
    }
).WithTags("Role");

app.MapGet(
    "/getRoles",
    async () =>
    {
        return await locator.GetRoles();
    }
).WithTags("Role");

app.MapPut(
    "/updateRole",
    async (UpdateRole updateRole) =>
    {
        await locator.UpdateRole(updateRole);
    }
).WithTags("Role");

app.MapDelete(
    "/deleteRole",
    async (int roleId) =>
    {
        await locator.DeleteRole(roleId);
    }
).WithTags("Role");

#endregion

#region User Role Endpoints

app.MapPost(
    "/addUserRole",
    async (int userId, int roleId) =>
    {
        return await locator.AddUserRole(userId, roleId);
    }
).WithTags("UserRole");

app.MapDelete(
    "/deleteUserRole",
    async (int userId, int roleId) =>
    {
        await locator.DeleteUserRole(userId, roleId);
    }
).WithTags("UserRole");

#endregion

#region Client User Endpoints

app.MapPost(
    "/addClientUser",
    async (int clientId, int userId) =>
    {
        return await locator.AddClientUser(clientId, userId);
    }
).WithTags("ClientUser");

app.MapDelete(
    "/deleteClientUser",
    async (int clientId, int userId) =>
    {
        await locator.DeleteClientUser(clientId, userId);
    }
).WithTags("ClientUser");

#endregion

#region Database Server Endpoints

app.MapPost(
    "/addDatabaseServer",
    async (AddDatabaseServer addDatabaseServer) =>
    {
        return await locator.AddDatabaseServer(addDatabaseServer);
    }
).WithTags("DatabaseServer");

app.MapGet(
    "/getDatabaseServer",
    async (int serverId) =>
    {
        return await locator.GetDatabaseServer(serverId);
    }
).WithTags("DatabaseServer");

app.MapGet(
    "/getDatabaseServers",
    async () =>
    {
        return await locator.GetDatabaseServers();
    }
).WithTags("DatabaseServer");

app.MapPut(
    "/updateDatabaseServer",
    async (UpdateDatabaseServer updateDatabaseServer) =>
    {
        await locator.UpdateDatabaseServer(updateDatabaseServer);
    }
).WithTags("DatabaseServer");

app.MapDelete(
    "/deleteDatabaseServer",
    async (int serverId) =>
    {
        await locator.DeleteDatabaseServer(serverId);
    }
).WithTags("DatabaseServer");

#endregion

#region Database Endpoints

app.MapPost(
    "/addDatabase",
    async (AddDatabase addDatabase) =>
    {
        return await locator.AddDatabase(addDatabase);
    }
).WithTags("Database");

app.MapGet(
    "/getDatabase",
    async (int databaseId) =>
    {
        return await locator.GetDatabase(databaseId);
    }
).WithTags("Database");

app.MapGet(
    "/getDatabases",
    async () =>
    {
        return await locator.GetDatabases();
    }
).WithTags("Database");

app.MapPut(
    "/updateDatabase",
    async (UpdateDatabase updateDatabase) =>
    {
        await locator.UpdateDatabase(updateDatabase);
    }
).WithTags("Database");

app.MapDelete(
    "/deleteDatabase",
    async (int databaseId) =>
    {
        await locator.DeleteDatabase(databaseId);
    }
).WithTags("Database");

#endregion

#region Database Type Endpoints

app.MapPost(
    "/addDatabaseType",
    async (string name) =>
    {
        return await locator.AddDatabaseType(name);
    }
).WithTags("DatabaseType");

app.MapGet(
    "/getDatabaseType",
    async (int databaseTypeId) =>
    {
        return await locator.GetDatabaseType(databaseTypeId);
    }
).WithTags("DatabaseType");

app.MapGet(
    "/getDatabaseTypes",
    async () =>
    {
        return await locator.GetDatabaseTypes();
    }
).WithTags("DatabaseType");

app.MapPut(
    "/updateDatabaseType",
    async (int databaseTypeId, string name) =>
    {
        await locator.UpdateDatabaseType(databaseTypeId, name);
    }
).WithTags("DatabaseType");

app.MapDelete(
    "/deleteDatabaseType",
    async (int databaseTypeId) =>
    {
        await locator.DeleteDatabaseType(databaseTypeId);
    }
).WithTags("DatabaseType");

#endregion

#region Connection Endpoints

app.MapPost(
    "/addConnection",
    async (int clientId, int databaseId) =>
    {
        return await locator.AddConnection(clientId, databaseId);
    }
).WithTags("Connection");

app.MapGet(
    "/getConnection",
    async (int connectionId) =>
    {
        return await locator.GetConnection(connectionId);
    }
).WithTags("Connection");

app.MapDelete(
    "/deleteConnection",
    async (int clientId, int databaseId) =>
    {
        await locator.DeleteConnection(clientId, databaseId);
    }
).WithTags("Connection");

#endregion

app.MapGet(
    "/getDataFromClientDatabase",
    async (int clientId, int databaseTypeId) =>
    {
        // get auth0Id from bearer token


        return await locator.GetConnection("1", clientId, databaseTypeId);
    }
).WithTags("A Connection To Client DB");


app.Run();

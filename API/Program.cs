using Dapper;
using Locator.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
        }
    );
    c.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer",
                    },
                },
                Array.Empty<string>()
            },
        }
    );
});

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
var configBuilder = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();
configBuilder.Build();

var domain = $"https://{builder.Configuration["Auth0:Domain"]}/";

builder
    .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = domain;
        options.Audience = builder.Configuration["Auth0:ApiAudience"];
    });

builder
    .Services.AddAuthorizationBuilder()
    .AddPolicy(
        "admin:read",
        p => p.RequireAuthenticatedUser().RequireClaim("permissions", "admin:read")
    );
builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

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

Locator.Locator locator =
    new(
        builder.Configuration["LocatorDb:ConnectionString"],
        builder.Configuration["Auth0:Domain"],
        builder.Configuration["Auth0:MachineToMachine:ClientID"],
        builder.Configuration["Auth0:MachineToMachine:ClientSecret"],
        builder.Configuration["Auth0:ApiIdentifier"],
        builder.Configuration["Auth0:ApiAudience"]
    );

// middle ware
app.Use(
    async (context, next) =>
    {
        // uncomment below line for getting user context from token
        context.Items["Auth0Id"] = locator.GetAuth0Id(context);

        await next();
    }
);

#region User Endpoints

app.MapPost(
        "/addUser",
        async (
            string firstName,
            string lastName,
            string emailAddress,
            string password,
            UserStatus userStatus
        ) => await locator.AddUser(firstName, lastName, emailAddress, password, userStatus)
    )
    .WithTags("User");

app.MapPost(
        "generatePasswordChangeTicket",
        async (string auth0Id, string redirectUrl) =>
            await locator.GeneratePasswordChangeTicket(auth0Id, redirectUrl)
    )
    .WithTags("User");

app.MapGet("/getUserLogs", async (string auth0Id) => await locator.GetUserLogs(auth0Id))
    .WithTags("User");

app.MapGet(
        "/getUsers",
        async (string keyword, int pageNumber, int pageSize) =>
            await locator.GetUsers(keyword, pageNumber, pageSize)
    )
    .WithTags("User");

app.MapPut(
        "/updateUser",
        async (
            int userId,
            string firstName,
            string lastName,
            string emailAddress,
            string password,
            UserStatus userStatus
        ) =>
            await locator.UpdateUser(
                userId,
                firstName,
                lastName,
                emailAddress,
                password,
                userStatus
            )
    )
    .WithTags("User");

app.MapPut(
        "/updateUserPassword",
        async (string auth0Id, string password) =>
            await locator.UpdateUserPassword(auth0Id, password)
    )
    .WithTags("User");

app.MapDelete("/deleteUser", async (int userId) => await locator.DeleteUser(userId))
    .WithTags("User");

#endregion

#region Client Endpoints

app.MapPost(
        "/addClient",
        async (string clientName, string clientCode, ClientStatus clientStatus) =>
            await locator.AddClient(clientName, clientCode, clientStatus)
    )
    .WithTags("Client");

app.MapGet(
        "/getClients",
        async (string search, int pageNumber, int pageSize) =>
            await locator.GetClients(search, pageNumber, pageSize)
    )
    .WithTags("Client");

app.MapPut(
        "/updateClient",
        async (int clientId, string clientName, string clientCode, ClientStatus clientStatus) =>
            await locator.UpdateClient(clientId, clientName, clientCode, clientStatus)
    )
    .WithTags("Client");

app.MapDelete("/deleteClient", async (int clientId) => await locator.DeleteClient(clientId))
    .WithTags("Client");

#endregion

#region Role Endpoints

app.MapPost(
        "/addRole",
        async (string roleName, string roleDescription) =>
            await locator.AddRole(roleName, roleDescription)
    )
    .WithTags("Role");

app.MapGet("/getRoles", async () => await locator.GetRoles()).WithTags("Role");

app.MapPut(
        "/updateRole",
        async (int roleId, string roleName, string roleDescription) =>
            await locator.UpdateRole(roleId, roleName, roleDescription)
    )
    .WithTags("Role");

app.MapDelete("/deleteRole", async (int roleId) => await locator.DeleteRole(roleId))
    .WithTags("Role");

#endregion

#region User Role Endpoints

app.MapPost(
        "/addUserRole",
        async (int userId, int roleId) => await locator.AddUserRole(userId, roleId)
    )
    .WithTags("UserRole");

app.MapDelete(
        "/deleteUserRole",
        async (int userId, int roleId) => await locator.DeleteUserRole(userId, roleId)
    )
    .WithTags("UserRole");

#endregion

#region Client User Endpoints

app.MapPost(
        "/addClientUser",
        async (int clientId, int userId) => await locator.AddClientUser(clientId, userId)
    )
    .WithTags("ClientUser");

app.MapDelete(
        "/deleteClientUser",
        async (int clientId, int userId) => await locator.DeleteClientUser(clientId, userId)
    )
    .WithTags("ClientUser");

#endregion

#region Database Server Endpoints

app.MapPost(
        "/addDatabaseServer",
        async (string databaseServerName, string databaseServerIpAddress) =>
            await locator.AddDatabaseServer(databaseServerName, databaseServerIpAddress)
    )
    .WithTags("DatabaseServer");

app.MapGet("/getDatabaseServers", async () => await locator.GetDatabaseServers())
    .WithTags("DatabaseServer");

app.MapPut(
        "/updateDatabaseServer",
        async (int databaseServerId, string databaseServerName, string databaseServerIpAddress) =>
            await locator.UpdateDatabaseServer(
                databaseServerId,
                databaseServerName,
                databaseServerIpAddress
            )
    )
    .WithTags("DatabaseServer");

app.MapDelete(
        "/deleteDatabaseServer",
        async (int serverId) => await locator.DeleteDatabaseServer(serverId)
    )
    .WithTags("DatabaseServer");

#endregion

#region Database Endpoints

app.MapPost(
        "/addDatabase",
        async (
            string databaseName,
            string databaseUser,
            int databaseServerId,
            int databaseTypeId,
            DatabaseStatus databaseStatus
        ) =>
            await locator.AddDatabase(
                databaseName,
                databaseUser,
                databaseServerId,
                databaseTypeId,
                databaseStatus
            )
    )
    .WithTags("Database");

app.MapGet(
        "/getDatabases",
        async (string searchText, int pageNumber, int pageSize) =>
            await locator.GetDatabases(searchText, pageNumber, pageSize)
    )
    .WithTags("Database");

app.MapPut(
        "/updateDatabase",
        async (
            int databaseId,
            string databaseName,
            string databaseUser,
            int databaseServerId,
            int databaseTypeId,
            DatabaseStatus databaseStatus
        ) =>
            await locator.UpdateDatabase(
                databaseId,
                databaseName,
                databaseUser,
                databaseServerId,
                databaseTypeId,
                databaseStatus
            )
    )
    .WithTags("Database");

app.MapDelete("/deleteDatabase", async (int databaseId) => await locator.DeleteDatabase(databaseId))
    .WithTags("Database");

#endregion

#region Database Type Endpoints

app.MapPost("/addDatabaseType", async (string name) => await locator.AddDatabaseType(name))
    .WithTags("DatabaseType");

app.MapGet("/getDatabaseTypes", async () => await locator.GetDatabaseTypes())
    .WithTags("DatabaseType");

app.MapPut(
        "/updateDatabaseType",
        async (int databaseTypeId, string name) =>
            await locator.UpdateDatabaseType(databaseTypeId, name)
    )
    .WithTags("DatabaseType");

app.MapDelete(
        "/deleteDatabaseType",
        async (int databaseTypeId) => await locator.DeleteDatabaseType(databaseTypeId)
    )
    .WithTags("DatabaseType");

#endregion

#region Connection Endpoints

app.MapPost(
        "/addConnection",
        async (int clientUserId, int databaseId) =>
            await locator.AddConnection(clientUserId, databaseId)
    )
    .WithTags("Connection");

app.MapDelete(
        "/deleteConnection",
        async (int connectionId) => await locator.DeleteConnection(connectionId)
    )
    .WithTags("Connection");

#endregion

#region Permission Endpoints

app.MapPost(
        "/addPermission",
        async (string permissionName, string permissionDescription) =>
            await locator.AddPermission(permissionName, permissionDescription)
    )
    .WithTags("Permission");

app.MapGet("/getPermissions", async () => await locator.GetPermissions()).WithTags("Permission");

app.MapPut(
        "/updatePermission",
        async (int permissionId, string permissionName, string permissionDescription) =>
            await locator.UpdatePermission(permissionId, permissionName, permissionDescription)
    )
    .WithTags("Permission");

app.MapDelete(
        "/deletePermission",
        async (int permissionId) => await locator.DeletePermission(permissionId)
    )
    .WithTags("Permission");

#endregion

#region Role Permission Endpoints

app.MapPost(
        "/addRolePermission",
        async (int roleId, int permissionId) =>
            await locator.AddRolePermission(roleId, permissionId)
    )
    .WithTags("Role Permission");

app.MapDelete(
        "/deleteRolePermission",
        async (int roleId, int permissionId) =>
            await locator.DeleteRolePermission(roleId, permissionId)
    )
    .WithTags("Role Permission");

#endregion

// user is signed in, hitting an endpoint with the admin:read permission, permission they setup through the library
app.MapGet(
        "/getStuff",
        async (HttpRequest request, int clientId, int databaseTypeId) =>
        {
            var auth0Id = request.HttpContext.Items["Auth0Id"]!.ToString();

            var db = await locator.GetConnection(auth0Id, clientId, databaseTypeId);

            // this would be database the library consumer would have setup, don't care what they put into it
            return await db.QueryAsync<dynamic>("SELECT * FROM dbo.Stuff");
        }
    )
    .WithTags("A Connection To Client DB");
    
app.Run();

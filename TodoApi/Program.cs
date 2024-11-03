using Dapper;
using Locator;
using Locator.Models.Write;
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
var configuration = configBuilder.Build();

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

LocatorLib locator =
    new(
        builder.Configuration["LocatorDb:ConnectionString"],
        builder.Configuration["Auth0:Domain"],
        builder.Configuration["Auth0:MachineToMachine:ClientID"],
        builder.Configuration["Auth0:MachineToMachine:ClientSecret"],
        builder.Configuration["Auth0:ApiIdentifier"],
        builder.Configuration["Auth0:ApiAudience"]
    );

app.Use(
    async (context, next) =>
    {
        //context.Items["Auth0Id"] = locator.GetAuth0Id(context);

        await next();
    }
);

#region User Endpoints

app.MapPost(
        "/addUser",
        async (AddUser addUser) =>
        {
            return await locator.AddUser(addUser);
        }
    )
    .WithTags("User");

app.MapGet(
        "/getUser",
        async (int userId) =>
        {
            return await locator.GetUser(userId);
        }
    )
    .WithTags("User");

app.MapGet(
        "/getUsers",
        async () =>
        {
            return await locator.GetUsers();
        }
    )
    .WithTags("User");

app.MapPut(
        "/updateUser",
        async (UpdateUser updateUser) =>
        {
            await locator.UpdateUser(updateUser);
        }
    )
    .WithTags("User");

app.MapDelete(
        "/deleteUser",
        async (string auth0Id) =>
        {
            await locator.DeleteUser(auth0Id);
        }
    )
    .WithTags("User");

#endregion

#region Client Endpoints

app.MapPost(
        "/addClient",
        async (AddClient addClient) =>
        {
            return await locator.AddClient(addClient);
        }
    )
    .WithTags("Client");

app.MapGet(
        "/getClient",
        async (int clientId) =>
        {
            return await locator.GetClient(clientId);
        }
    )
    .WithTags("Client");

app.MapGet(
        "/getClients",
        async () =>
        {
            return await locator.GetClients();
        }
    )
    .WithTags("Client");

app.MapPut(
        "/updateClient",
        async (UpdateClient updateClient) =>
        {
            await locator.UpdateClient(updateClient);
        }
    )
    .WithTags("Client");

app.MapDelete(
        "/deleteClient",
        async (int clientId) =>
        {
            await locator.DeleteClient(clientId);
        }
    )
    .WithTags("Client");

#endregion

#region Role Endpoints

app.MapPost(
        "/addRole",
        async (AddRole addRole) =>
        {
            return await locator.AddRole(addRole);
        }
    )
    .WithTags("Role");

app.MapGet(
        "/getRole",
        async (int roleId) =>
        {
            return await locator.GetRole(roleId);
        }
    )
    .WithTags("Role");

app.MapGet(
        "/getRoles",
        async () =>
        {
            return await locator.GetRoles();
        }
    )
    .WithTags("Role");

app.MapPut(
        "/updateRole",
        async (UpdateRole updateRole) =>
        {
            await locator.UpdateRole(updateRole);
        }
    )
    .WithTags("Role");

app.MapDelete(
        "/deleteRole",
        async (int roleId) =>
        {
            await locator.DeleteRole(roleId);
        }
    )
    .WithTags("Role");

#endregion

#region User Role Endpoints

app.MapPost(
        "/addUserRole",
        async (int userId, int roleId) =>
        {
            return await locator.AddUserRole(userId, roleId);
        }
    )
    .WithTags("UserRole");

app.MapDelete(
        "/deleteUserRole",
        async (int userId, int roleId) =>
        {
            await locator.DeleteUserRole(userId, roleId);
        }
    )
    .WithTags("UserRole");

#endregion

#region Client User Endpoints

app.MapPost(
        "/addClientUser",
        async (int clientId, int userId) =>
        {
            return await locator.AddClientUser(clientId, userId);
        }
    )
    .WithTags("ClientUser");

app.MapDelete(
        "/deleteClientUser",
        async (int clientId, int userId) =>
        {
            await locator.DeleteClientUser(clientId, userId);
        }
    )
    .WithTags("ClientUser");

#endregion

#region Database Server Endpoints

app.MapPost(
        "/addDatabaseServer",
        async (AddDatabaseServer addDatabaseServer) =>
        {
            return await locator.AddDatabaseServer(addDatabaseServer);
        }
    )
    .WithTags("DatabaseServer");

app.MapGet(
        "/getDatabaseServer",
        async (int serverId) =>
        {
            return await locator.GetDatabaseServer(serverId);
        }
    )
    .WithTags("DatabaseServer");

app.MapGet(
        "/getDatabaseServers",
        async () =>
        {
            return await locator.GetDatabaseServers();
        }
    )
    .WithTags("DatabaseServer");

app.MapPut(
        "/updateDatabaseServer",
        async (UpdateDatabaseServer updateDatabaseServer) =>
        {
            await locator.UpdateDatabaseServer(updateDatabaseServer);
        }
    )
    .WithTags("DatabaseServer");

app.MapDelete(
        "/deleteDatabaseServer",
        async (int serverId) =>
        {
            await locator.DeleteDatabaseServer(serverId);
        }
    )
    .WithTags("DatabaseServer");

#endregion

#region Database Endpoints

app.MapPost(
        "/addDatabase",
        async (AddDatabase addDatabase) =>
        {
            return await locator.AddDatabase(addDatabase);
        }
    )
    .WithTags("Database");

app.MapGet(
        "/getDatabase",
        async (int databaseId) =>
        {
            return await locator.GetDatabase(databaseId);
        }
    )
    .WithTags("Database");

app.MapGet(
        "/getDatabases",
        async () =>
        {
            return await locator.GetDatabases();
        }
    )
    .WithTags("Database");

app.MapPut(
        "/updateDatabase",
        async (UpdateDatabase updateDatabase) =>
        {
            await locator.UpdateDatabase(updateDatabase);
        }
    )
    .WithTags("Database");

app.MapDelete(
        "/deleteDatabase",
        async (int databaseId) =>
        {
            await locator.DeleteDatabase(databaseId);
        }
    )
    .WithTags("Database");

#endregion

#region Database Type Endpoints

app.MapPost(
        "/addDatabaseType",
        async (string name) =>
        {
            return await locator.AddDatabaseType(name);
        }
    )
    .WithTags("DatabaseType");

app.MapGet(
        "/getDatabaseType",
        async (int databaseTypeId) =>
        {
            return await locator.GetDatabaseType(databaseTypeId);
        }
    )
    .WithTags("DatabaseType");

app.MapGet(
        "/getDatabaseTypes",
        async () =>
        {
            return await locator.GetDatabaseTypes();
        }
    )
    .WithTags("DatabaseType");

app.MapPut(
        "/updateDatabaseType",
        async (int databaseTypeId, string name) =>
        {
            await locator.UpdateDatabaseType(databaseTypeId, name);
        }
    )
    .WithTags("DatabaseType");

app.MapDelete(
        "/deleteDatabaseType",
        async (int databaseTypeId) =>
        {
            await locator.DeleteDatabaseType(databaseTypeId);
        }
    )
    .WithTags("DatabaseType");

#endregion

#region Connection Endpoints

app.MapPost(
        "/addConnection",
        async (int clientUserId, int databaseId) =>
        {
            return await locator.AddConnection(clientUserId, databaseId);
        }
    )
    .WithTags("Connection");

app.MapGet(
        "/getConnection",
        async (int connectionId) =>
        {
            return await locator.GetConnection(connectionId);
        }
    )
    .WithTags("Connection");

app.MapDelete(
        "/deleteConnection",
        async (int clientId, int databaseId) =>
        {
            await locator.DeleteConnection(clientId, databaseId);
        }
    )
    .WithTags("Connection");

#endregion

#region Permission Endpoints

app.MapPost(
        "/addPermission",
        async (string permissionName, string permissionDescription) =>
        {
            await locator.AddPermission(permissionName, permissionDescription);
        }
    )
    .WithTags("Permission");

#endregion

#region Role Permission Endpoints

app.MapPost(
        "/addRolePermission",
        async (int roleId, int permissionId) =>
        {
            await locator.AddRolePermission(roleId, permissionId);
        }
    )
    .WithTags("Role Permission");

#endregion


app.MapGet(
        "/getStuff",
        async (HttpRequest request, int clientId, int databaseTypeId) =>
        {
            var auth0Id = request.HttpContext.Items["Auth0Id"]!.ToString();

            var db = await locator.GetConnection(auth0Id, clientId, databaseTypeId);

            return await db.QueryAsync<dynamic>("SELECT * FROM dbo.Stuff");
        }
    )
    .WithTags("A Connection To Client DB")
    .RequireAuthorization("admin:read");

app.Run();

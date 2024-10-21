using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Locator.Models;
using Locator.Repositories;
using ZiggyCreatures.Caching.Fusion;

namespace Locator.Services;

public partial class LocatorService
{
    private readonly LocatorRepository _locatorRepo;
    private readonly Auth0Service _auth0Service;
    private readonly IDbConnection _clientDb;
    private readonly int _clientId;
    private readonly string _clientCode;
    private readonly string _auth0Id;
    private readonly int _userId;
    private readonly IFusionCache _cache;
    private readonly ValidationHelper _validationHelper;

    public LocatorService(
        IHttpContextAccessor httpContextAccessor,
        IFusionCache cache,
        Auth0Service auth0Service,
        LocatorRepository locatorRepo,
        AuthenticationHelper authenticationHelper,
        ValidationHelper validationHelper
    )
    {
        if (authenticationHelper.Auth0Id(httpContextAccessor.HttpContext) is not { } auth0Id)
            return;
        var connection = cache.GetOrSet(
            $"ClientDb:{auth0Id}",
            locatorRepo.GetConnection(auth0Id, (int)DatabaseType.Client),
            TimeSpan.FromHours(8)
        );

        _locatorRepo = locatorRepo;
        _auth0Service = auth0Service;
        _auth0Id = auth0Id;
        _userId = connection.UserId;
        _clientId = connection.ClientId;
        _clientCode = connection.ClientCode;
        _clientDb = new SqlConnection(
            $@"
            Server={connection.DatabaseServer};
            User Id={connection.DatabaseUser};
            Password={connection.DatabaseUserPassword};
            Database={connection.DatabaseName};"
        );
        _cache = cache;
        _validationHelper = validationHelper;
    }

    public string Auth0Id()
    {
        return _auth0Id;
    }

    public int UserId()
    {
        return _userId;
    }

    public int ClientId()
    {
        return _clientId;
    }

    public string ClientCode()
    {
        return _clientCode;
    }

    public IDbConnection ClientDb()
    {
        return _clientDb;
    }

    private UserStatus GetUserStatus(string auth0Id)
    {
        return _locatorRepo.GetUserStatus(auth0Id);
    }

    public Connection GetConnection(string auth0Id, int databaseTypeId)
    {
        var clientStatus = GetClientStatus(auth0Id);
        var userStatus = GetUserStatus(auth0Id);

        if (clientStatus == ClientStatus.Active && userStatus == UserStatus.Active)
            return _locatorRepo.GetConnection(auth0Id, databaseTypeId);
        else
            throw new ValidationException("unauthorized");
    }

    public async Task<Client> GetClient(string clientCode)
    {
        return await _locatorRepo.GetClient(clientCode);
    }

    public async Task<PagedList<Client>> GetClients(string keyword, int pageNumber, int pageSize)
    {
        return await _locatorRepo.GetClients(keyword, pageNumber, pageSize);
    }

    public async Task<List<KeyValuePair<int, string>>> GetClientList()
    {
        return await _locatorRepo.GetClientList();
    }

    public async Task<Client> UpdateClient(UpdateClient updateClient)
    {
        await _locatorRepo.UpdateClient(updateClient, UserId());

        var auth0IDs = await _locatorRepo.GetAuth0Ids(updateClient.ClientCode);

        foreach (var auth0Id in auth0IDs)
            await _cache.RemoveAsync($"ClientDb:{auth0Id}");

        return await GetClient(updateClient.ClientCode);
    }

    public async Task<bool> IsClientActive(string clientCode)
    {
        return await _locatorRepo.IsClientActive(clientCode);
    }

    // public async Task<Client> AddClient(AddClient addClient)
    // {
    //     var dbName = addClient.ClientName.Replace(" ", "_") + "_DB";
    //     var dbUser = addClient.ClientName.Replace(" ", "_") + "_App";

    //     // hash this one day before inserting as plain text?
    //     var dbPassword = Guid.NewGuid().ToString();

    //     var client = await _locatorRepo.AddClient(addClient, _userId);

    //     DynamicParameters parameters = new();
    //     parameters.Add("ClientID", client.ClientId);
    //     parameters.Add("DatabaseName", dbName);
    //     parameters.Add("DatabaseUser", dbUser);
    //     parameters.Add("DatabaseUserPassword", dbPassword);
    //     parameters.Add("DatabaseServerID", addClient.DatabaseServerId);
    //     parameters.Add("DatabaseTypeID", DatabaseType.Client);
    //     parameters.Add("UserID", _userId);

    //     Console.WriteLine($"Inserting locator records for {dbName}...");

    //     var databaseId = await _locatorRepo.QuerySingleAsync<int>(
    //         @$"
    //         insert into dbo.[Database]
    //         (
    //             DatabaseName,
    //             DatabaseUser,
    //             DatabaseUserPassword,
    //             DatabaseServerID,
    //             DatabaseTypeID,
    //             CreateDate,
    //             CreateByID,
    //             ModifyDate,
    //             ModifyByID
    //         )
    //         values
    //         (
    //             @DatabaseName,
    //             @DatabaseUser,
    //             @DatabaseUserPassword,
    //             @DatabaseServerID,
    //             @DatabaseTypeID,
    //             getutcdate(),
    //             @UserID,
    //             getutcdate(),
    //             @UserID
    //         )

    //         declare @DatabaseID int
    //         set @DatabaseID = scope_identity()

    //         insert into dbo.ClientConnection
    //         (
    //             ClientID,
    //             DatabaseID,
    //             CreateDate,
    //             CreateByID,
    //             ModifyDate,
    //             ModifyByID
    //         )
    //         values
    //         (
    //             @ClientID,
    //             @DatabaseID,
    //             getutcdate(),
    //             @UserID,
    //             getutcdate(),
    //             @UserID
    //         );

    //         select @DatabaseID",
    //         parameters
    //     );

    //     string databaseServerName = await locatorDb.QueryFirstOrDefaultAsync<string>(
    //         "select DatabaseServerName from dbo.DatabaseServer where DatabaseServerID = @DatabaseServerID",
    //         new { dataBaseServerId }
    //     );

    //     Console.WriteLine($"Inserted Locator records for {dbName}!");
    //     Console.WriteLine($"Creating {dbName}...");

    //     // use database back up and restore instead of schema zen, include seed data

    //     Console.WriteLine($"{dbName} created!");
    //     Console.WriteLine($"Creating SQL Login for {dbUser}...");

    //     await locatorDb.ExecuteAsync(
    //         @$"CREATE LOGIN {dbUser} WITH PASSWORD = '{dbPassword}'",
    //         new { dbUser, dbPassword }
    //     );

    //     Console.WriteLine($"Created SQL Login for {dbUser}!");
    //     Console.WriteLine($"Creating {dbUser} user for {dbName} and assigning roles...");

    //     await locatorDb.ExecuteAsync(
    //         @$"
    //             use {dbName}
    //             create user {dbUser} for login {dbUser}

    //             alter role [db_datareader] add member {dbUser}

    //             alter role [db_datawriter] add member {dbUser}

    //             grant control on type::[dbo].[Filter_Int] to {dbUser} as [dbo]

    //             grant control on type::[dbo].[Filter_String] to {dbUser} as [dbo]",
    //         new { dbUser, dbName }
    //     );

    //     Console.WriteLine($"Created {dbUser} user for {dbName} and assigned roles!");

    //     var clientDb = DatabaseHelper.CreateClientConnection(
    //         clientCode,
    //         _locatorConnectionString,
    //         DatabaseType.Client
    //     );

    //     Console.WriteLine($"Activating {dbName}...");

    //     await locatorDb.ExecuteAsync(
    //         "update dbo.[Database] set IsActive=1 where DatabaseID = @DatabaseID",
    //         new { databaseId }
    //     );

    //     Console.WriteLine($"Activated {dbName}!");
    // }

    private ClientStatus GetClientStatus(string auth0Id)
    {
        return _locatorRepo.GetClientStatus(auth0Id);
    }

    public async Task<ClientUser> GetClientUser(string auth0Id)
    {
        return await _locatorRepo.GetClientUser(auth0Id);
    }

    public async Task<User> GetUser(string auth0Id)
    {
        return await _locatorRepo.GetUser(auth0Id);
    }

    public async Task<User> GetUser(int userId)
    {
        return await _locatorRepo.GetUser(userId);
    }

    public async Task<PagedList<User>> GetUsers(
        int clientId,
        string search,
        int pageNumber,
        int pageSize
    )
    {
        return await _locatorRepo.GetUsers(clientId, search, pageNumber, pageSize);
    }

    public async Task<User> AddUser(AddUser addUser)
    {
        if (!_validationHelper.IsValidEmail(addUser.EmailAddress))
            throw new ValidationException("Invalid email address");

        if (await _locatorRepo.CheckEmailExists(addUser.EmailAddress))
            throw new ValidationException("Email is already taken");

        var clientCode = await GetClientCode(addUser.ClientId);

        var accessToken = await _auth0Service.GetAccessToken();
        var auth0Id = await _auth0Service.CreateUser(
            accessToken,
            addUser.EmailAddress,
            addUser.FirstName,
            addUser.LastName,
            clientCode
        );

        var allRoles = await _locatorRepo.GetRoles();

        var userRoles = allRoles.Where(x => addUser.RoleIds.Contains(x.RoleId)).ToList();
        foreach (var role in userRoles)
            await _auth0Service.AssignUserToRole(accessToken, auth0Id, role.Auth0RoleId);

        var passwordChangeTicket = await _auth0Service.GeneratePasswordChangeTicket(
            accessToken,
            auth0Id
        );

        // _emailService.Send(
        //     addUser.EmailAddress,
        //     (int)EmailTemplate.NewAccount,
        //     addUser,
        //     passwordChangeTicket.URL
        // );

        return await _locatorRepo.AddUser(addUser, auth0Id, UserId());
    }

    public async Task<List<KeyValuePair<int, string>>> GetRoleList()
    {
        return await _locatorRepo.GetRoleList();
    }

    public async Task<User> DeleteUser(string auth0Id)
    {
        var accessToken = await _auth0Service.GetAccessToken();

        await _auth0Service.DeleteUser(accessToken, auth0Id);

        return await _locatorRepo.DeleteUser(auth0Id);
    }

    public async Task<User> UpdateProfile(UpdateProfile updateProfile, string auth0Id)
    {
        if (!_validationHelper.IsValidEmail(updateProfile.EmailAddress))
            throw new ValidationException("Invalid email address");

        var currentEmail = await GetUserEmail(UserId());
        if (currentEmail != updateProfile.EmailAddress)
        {
            if (await _locatorRepo.CheckEmailExists(updateProfile.EmailAddress))
                throw new ValidationException("Email is already taken");
        }

        var accessToken = await _auth0Service.GetAccessToken();

        await _auth0Service.UpdateUser(
            accessToken,
            auth0Id,
            updateProfile.FirstName,
            updateProfile.LastName,
            updateProfile.EmailAddress
        );

        return await _locatorRepo.UpdateProfile(updateProfile, auth0Id, UserId());
    }

    public async Task<User> UpdateUser(UpdateUser updateUser)
    {
        if (!_validationHelper.IsValidEmail(updateUser.EmailAddress))
            throw new ValidationException("Invalid email address");

        var currentEmail = await GetUserEmail(updateUser.UserId);
        if (currentEmail != updateUser.EmailAddress)
        {
            if (await _locatorRepo.CheckEmailExists(updateUser.EmailAddress))
                throw new ValidationException("Email is already taken");
        }

        var accessToken = await _auth0Service.GetAccessToken();

        await _auth0Service.UpdateUser(
            accessToken,
            updateUser.Auth0Id,
            updateUser.FirstName,
            updateUser.LastName,
            updateUser.EmailAddress,
            updateUser.UserStatusId != (int)UserStatus.Active
        );

        var roles = await _locatorRepo.GetRoles();
        foreach (var role in roles)
            await _auth0Service.RemoveUserFromRole(
                accessToken,
                updateUser.Auth0Id,
                role.Auth0RoleId
            );

        var userRoles = roles.Where(x => updateUser.RoleIds.Contains(x.RoleId)).ToList();
        foreach (var role in userRoles)
            await _auth0Service.AssignUserToRole(accessToken, updateUser.Auth0Id, role.Auth0RoleId);

        await _cache.RemoveAsync($"ClientDb:{updateUser.Auth0Id}");

        return await _locatorRepo.UpdateUser(updateUser, UserId());
    }

    public async Task<List<UserLog>> GetUserLogs(string auth0Id)
    {
        var accessToken = await _auth0Service.GetAccessToken();

        return await _auth0Service.GetUserLogs(accessToken, auth0Id);
    }

    public async Task<User> UpdateUserPassword(string password, string auth0Id)
    {
        var passwordRegex = PasswordRegex();
        if (!passwordRegex.IsMatch(password))
        {
            throw new ValidationException(
                "Password must contain at least 8 characters, one uppercase letter, one lowercase letter, one number, and one special character"
            );
        }

        var accessToken = await _auth0Service.GetAccessToken();

        await _auth0Service.UpdateUserPassword(accessToken, auth0Id, password);

        return await _locatorRepo.GetUser(auth0Id);
    }

    public async Task<string> GeneratePasswordChangeTicket(string auth0Id)
    {
        var accessToken = await _auth0Service.GetAccessToken();

        return await _auth0Service.GeneratePasswordChangeTicket(accessToken, auth0Id); // outputs url
    }

    public async Task<List<KeyValuePair<int, string>>> GetDatabaseServers()
    {
        return await _locatorRepo.GetDatabaseServers();
    }

    public async Task<List<Database>> GetDatabases(string clientCode)
    {
        return await _locatorRepo.GetDatabases(clientCode);
    }

    private async Task<string> GetClientCode(int clientId)
    {
        return await _locatorRepo.GetClientCode(clientId);
    }

    private async Task<string> GetUserEmail(int userId)
    {
        return await _locatorRepo.GetUserEmail(userId);
    }

    [GeneratedRegex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*]).{8,}$")]
    private static partial Regex PasswordRegex();
}

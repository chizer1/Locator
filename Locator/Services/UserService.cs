using Locator.Models.Read;
using Locator.Models.Write;
using Locator.Repositories;

namespace Locator.Services;

internal class UserService(
    UserRepository userRepository,
    RoleService roleService,
    Auth0Service auth0Service
)
{
    public async Task<int> AddUser(AddUser addUser)
    {
        var accessToken = await auth0Service.GetAccessToken();

        var auth0Id = await auth0Service.CreateUser(
            accessToken,
            addUser.EmailAddress,
            addUser.FirstName,
            addUser.LastName,
            addUser.Password
        );

        var userId = await userRepository.AddUser(
            addUser.FirstName,
            addUser.LastName,
            addUser.EmailAddress,
            addUser.UserStatus,
            auth0Id
        );

        foreach (var role in addUser.Roles)
            await roleService.AddUserRole(accessToken, userId, role.RoleId);

        return userId;
    }

    public async Task<User> GetUser(string auth0Id)
    {
        return await userRepository.GetUser(auth0Id);
    }

    public async Task<User> GetUser(int userId)
    {
        return await userRepository.GetUser(userId);
    }

    public async Task<List<User>> GetUsers()
    {
        return await userRepository.GetUsers();
    }

    public async Task<PagedList<User>> GetUsers(string searchText, int page, int pageSize)
    {
        return await userRepository.GetUsers(searchText, page, pageSize);
    }

    public async Task<List<UserLog>> GetUserLogs(string auth0Id)
    {
        var accessToken = await auth0Service.GetAccessToken();

        return await auth0Service.GetUserLogs(accessToken, auth0Id);
    }

    public async Task<List<UserLog>> GetUserLogs(int userId)
    {
        var accessToken = await auth0Service.GetAccessToken();

        var user = await userRepository.GetUser(userId);

        return await auth0Service.GetUserLogs(accessToken, user.Auth0Id);
    }

    public async Task UpdateUser(UpdateUser updateUser)
    {
        var accessToken = await auth0Service.GetAccessToken();
        var user = await userRepository.GetUser(updateUser.UserId);

        await auth0Service.UpdateUser(
            accessToken,
            user.Auth0Id,
            updateUser.FirstName,
            updateUser.LastName,
            updateUser.EmailAddress,
            updateUser.UserStatus == UserStatus.Active
        );

        foreach (var role in user.Roles)
            await roleService.DeleteUserRole(accessToken, user.UserId, role.RoleId);

        foreach (var role in updateUser.Roles)
            await roleService.AddUserRole(accessToken, user.UserId, role.RoleId);

        await userRepository.UpdateUser(updateUser);
    }

    public async Task UpdateUserPassword(string password, string auth0Id)
    {
        var accessToken = await auth0Service.GetAccessToken();

        await auth0Service.UpdateUserPassword(accessToken, auth0Id, password);
    }

    public async Task<string> GeneratePasswordChangeTicket(string auth0Id)
    {
        var accessToken = await auth0Service.GetAccessToken();

        return await auth0Service.GeneratePasswordChangeTicket(accessToken, auth0Id, "");
    }

    public async Task DeleteUser(string auth0Id)
    {
        var accessToken = await auth0Service.GetAccessToken();

        var user = await userRepository.GetUser(auth0Id);
        foreach (var role in user.Roles)
            await roleService.DeleteUserRole(accessToken, user.UserId, role.RoleId);

        await auth0Service.DeleteUser(accessToken, auth0Id);
        await userRepository.DeleteUser(auth0Id);
    }

    public async Task DeleteUser(int userId)
    {
        var accessToken = await auth0Service.GetAccessToken();

        var user = await userRepository.GetUser(userId);
        foreach (var role in user.Roles)
            await roleService.DeleteUserRole(accessToken, user.UserId, role.RoleId);

        await auth0Service.DeleteUser(accessToken, user.Auth0Id);
        await userRepository.DeleteUser(user.UserId);
    }
}

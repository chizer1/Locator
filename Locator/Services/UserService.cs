using Locator.Models.Read;
using Locator.Repositories;

namespace Locator.Services;

internal class UserService(
    UserRepository userRepository,
    //RoleService roleService,
    Auth0Service auth0Service
)
{
    public async Task<int> AddUser(
        string firstName,
        string lastName,
        string emailAddress,
        List<Role> roles,
        UserStatus userStatus
    )
    {
        var accessToken = await auth0Service.GetAccessToken();
        var auth0Id = await auth0Service.CreateUser(accessToken, emailAddress, firstName, lastName);
        var userId = await userRepository.AddUser(
            firstName,
            lastName,
            emailAddress,
            userStatus,
            auth0Id
        );

        // foreach (var role in roles)
        // {
        //     await roleService.AddUserRole(userId, role);
        //     await auth0Service.AssignUserToRole(accessToken, auth0Id, role.Auth0RoleId);
        // }

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

    public async Task UpdateUser(
        int userId,
        string auth0Id,
        string firstName,
        string lastName,
        string emailAddress,
        UserStatus userStatus,
        List<Role> roles
    )
    {
        var accessToken = await auth0Service.GetAccessToken();

        await auth0Service.UpdateUser(
            accessToken,
            auth0Id,
            firstName,
            lastName,
            emailAddress,
            userStatus == UserStatus.Active
        );

        // add/remove roles in this block here

        await userRepository.UpdateUser(userId, firstName, lastName, emailAddress, userStatus);
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
        // foreach (var role in user.Roles)
        //     await roleService.DeleteUserRole(user.UserId, role.RoleId);

        await auth0Service.DeleteUser(accessToken, auth0Id);
        await userRepository.DeleteUser(auth0Id);
    }
}

using Locator.Models;
using Locator.Repositories;

namespace Locator.Services;

internal class UserService(
    UserRepository userRepository,
    RoleRepository roleRepository,
    Auth0Service auth0Service
)
{
    public async Task<int> AddUser(
        string firstName,
        string lastName,
        string emailAddress,
        int[] roleIds,
        UserStatus userStatus,
        int clientId
    )
    {
        // var accessToken = await auth0Service.GetAccessToken();
        // var auth0Id = await auth0Service.CreateUser(
        //     accessToken,
        //     emailAddress,
        //     firstName,
        //     lastName,
        //     clientId.ToString()
        // );

        // var allRoles = await roleRepository.GetRoles();

        // var userRoles = allRoles.Where(x => roleIds.Contains(x.RoleId)).ToList();
        // foreach (var role in userRoles)
        //     await auth0Service.AssignUserToRole(accessToken, auth0Id, role.Auth0RoleId);

        return await userRepository.AddUser(
            firstName,
            lastName,
            emailAddress,
            roleIds,
            userStatus,
            clientId,
            "auth0Id"
        );
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

    public async Task<PagedList<User>> GetUsers(
        int clientId,
        string searchText,
        int page,
        int pageSize
    )
    {
        return await userRepository.GetUsers(clientId, searchText, page, pageSize);
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
        int[] roleIds,
        int clientId
    )
    {
        // var currentEmail = await GetUserEmail(updateUser.UserId);
        // if (currentEmail != updateUser.EmailAddress)
        // {
        //     if (await _locatorRepo.CheckEmailExists(updateUser.EmailAddress))
        //         throw new ValidationException("Email is already taken");
        // }

        var accessToken = await auth0Service.GetAccessToken();

        await auth0Service.UpdateUser(
            accessToken,
            auth0Id,
            firstName,
            lastName,
            emailAddress,
            true
        );

        var roles = await roleRepository.GetRoles();
        foreach (var role in roles)
            await auth0Service.RemoveUserFromRole(accessToken, auth0Id, role.Auth0RoleId);

        var userRoles = roles.Where(x => roleIds.Contains(x.RoleId)).ToList();
        foreach (var role in userRoles)
            await auth0Service.AssignUserToRole(accessToken, auth0Id, role.Auth0RoleId);

        await userRepository.UpdateUser(
            userId,
            firstName,
            lastName,
            emailAddress,
            userStatus,
            clientId,
            roleIds
        );
    }

    public async Task UpdateUserPassword(string password, string auth0Id)
    {
        // var passwordRegex = PasswordRegex();
        // if (!passwordRegex.IsMatch(password))
        // {
        //     throw new ValidationException(
        //         "Password must contain at least 8 characters, one uppercase letter, one lowercase letter, one number, and one special character"
        //     );
        // }

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

        await auth0Service.DeleteUser(accessToken, auth0Id);

        await userRepository.DeleteUser(auth0Id);
    }
}

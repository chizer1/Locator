using Locator.Common.Models;
using Locator.Db;
using Locator.Domain;
using Microsoft.EntityFrameworkCore;

namespace Locator.Features.Users;

internal class UserRepository(DbContext locatorDb) : IUserRepository
{
    public async Task<int> AddUser(
        string firstName,
        string lastName,
        string emailAddress,
        Status userStatus,
        string auth0Id
    )
    {
        var user = new UserEntity
        {
            FirstName = firstName,
            LastName = lastName,
            EmailAddress = emailAddress,
            UserStatusId = (byte)userStatus,
            Auth0Id = auth0Id,
        };

        await locatorDb.Set<UserEntity>().AddAsync(user);
        await locatorDb.SaveChangesAsync();
        return user.UserId;
    }

    public async Task<User> GetUserByAuth0Id(string auth0Id)
    {
        var userEntity = await locatorDb
            .Set<UserEntity>()
            .FirstOrDefaultAsync(u => u.Auth0Id == auth0Id);

        if (userEntity == null)
            return null;

        return new User(
            userEntity.UserId,
            userEntity.Auth0Id,
            userEntity.FirstName,
            userEntity.LastName,
            userEntity.EmailAddress,
            (Status)userEntity.UserStatusId
        );
    }

    public async Task<User> GetUser(string emailAddress)
    {
        var userEntity = await locatorDb
            .Set<UserEntity>()
            .FirstOrDefaultAsync(u => u.EmailAddress == emailAddress);

        if (userEntity == null)
            return null;

        return new User(
            userEntity.UserId,
            userEntity.Auth0Id,
            userEntity.FirstName,
            userEntity.LastName,
            userEntity.EmailAddress,
            (Status)userEntity.UserStatusId
        );
    }

    public async Task<User> GetUser(int userId)
    {
        var userEntity =
            await locatorDb.Set<UserEntity>().FirstOrDefaultAsync(u => u.UserId == userId)
            ?? throw new KeyNotFoundException("User not found.");

        return new User(
            userEntity.UserId,
            userEntity.Auth0Id,
            userEntity.FirstName,
            userEntity.LastName,
            userEntity.EmailAddress,
            (Status)userEntity.UserStatusId
        );
    }

    public async Task<List<User>> GetUsers()
    {
        var userEntities = await locatorDb.Set<UserEntity>().OrderBy(u => u.LastName).ToListAsync();

        return userEntities
            .Select(userEntity => new User(
                userEntity.UserId,
                userEntity.Auth0Id,
                userEntity.FirstName,
                userEntity.LastName,
                userEntity.EmailAddress,
                (Status)userEntity.UserStatusId
            ))
            .ToList();
    }

    public async Task<PagedList<User>> GetUsers(string keyword, int pageNumber, int pageSize)
    {
        var query = locatorDb.Set<UserEntity>().AsQueryable();

        if (!string.IsNullOrEmpty(keyword))
        {
            var hasKeyword = (string compare) =>
                compare.Contains(keyword, StringComparison.CurrentCultureIgnoreCase);
            query = query.Where(u =>
                hasKeyword(u.FirstName) || hasKeyword(u.LastName) || hasKeyword(u.EmailAddress)
            );
        }

        var totalCount = await query.CountAsync();

        var users = await query
            .OrderBy(u => u.LastName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedList<User>(
            users.Select(userEntity => new User(
                userEntity.UserId,
                userEntity.Auth0Id,
                userEntity.FirstName,
                userEntity.LastName,
                userEntity.EmailAddress,
                (Status)userEntity.UserStatusId
            )),
            totalCount,
            pageNumber,
            pageSize,
            (int)Math.Ceiling((double)totalCount / pageSize)
        );
    }

    public async Task UpdateUser(
        int userId,
        string firstName,
        string lastName,
        string emailAddress,
        Status userStatus
    )
    {
        var userEntity =
            await locatorDb.Set<UserEntity>().FindAsync(userId)
            ?? throw new KeyNotFoundException($"User with ID {userId} not found.");

        userEntity.FirstName = firstName;
        userEntity.LastName = lastName;
        userEntity.EmailAddress = emailAddress;
        userEntity.UserStatusId = (byte)userStatus;

        locatorDb.Set<UserEntity>().Update(userEntity);
        await locatorDb.SaveChangesAsync();
    }

    public async Task DeleteUser(int userId)
    {
        var user =
            await locatorDb.Set<UserEntity>().FindAsync(userId)
            ?? throw new KeyNotFoundException($"User with ID {userId} not found.");

        locatorDb.Set<UserEntity>().Remove(user);
        await locatorDb.SaveChangesAsync();
    }

    public async Task DeleteUser(string auth0Id)
    {
        var user =
            await locatorDb.Set<User>().FirstOrDefaultAsync(u => u.Auth0Id == auth0Id)
            ?? throw new KeyNotFoundException($"User with Auth0Id {auth0Id} not found.");

        locatorDb.Set<User>().Remove(user);
        await locatorDb.SaveChangesAsync();
    }
}

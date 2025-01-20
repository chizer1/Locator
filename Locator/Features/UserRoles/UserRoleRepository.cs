using Locator.Db;
using Locator.Domain;
using Microsoft.EntityFrameworkCore;

namespace Locator.Features.UserRoles
{
    internal class UserRoleRepository(DbContext locatorDb) : IUserRoleRepository
    {
        public async Task<int> AddUserRole(int userId, int roleId)
        {
            var userRole = new UserRoleEntity { UserId = userId, RoleId = roleId };

            locatorDb.Add(userRole);
            await locatorDb.SaveChangesAsync();

            return userRole.UserRoleId; // Assuming UserRoleId is generated on save
        }

        public async Task DeleteUserRole(int userId, int roleId)
        {
            var userRole = await locatorDb
                .Set<UserRoleEntity>()
                .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

            if (userRole != null)
            {
                locatorDb.Remove(userRole);
                await locatorDb.SaveChangesAsync();
            }
        }
    }
}

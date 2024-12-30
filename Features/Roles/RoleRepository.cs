using Locator.Db;
using Locator.Domain;
using Microsoft.EntityFrameworkCore;

namespace Locator.Features.Roles
{
    internal class RoleRepository(DbContext locatorDb) : IRoleRepository
    {
        public async Task<int> AddRole(string auth0RoleId, string roleName, string roleDescription)
        {
            var role = new RoleEntity
            {
                Auth0RoleId = auth0RoleId,
                Name = roleName,
                Description = roleDescription,
            };

            locatorDb.Add(role);
            await locatorDb.SaveChangesAsync();

            return role.RoleId;
        }

        public async Task<Role> GetRole(int roleId)
        {
            var roleEntity = await locatorDb
                .Set<RoleEntity>()
                .FirstOrDefaultAsync(r => r.RoleId == roleId);

            if (roleEntity == null)
                return null;

            return new Role(
                roleEntity.RoleId,
                roleEntity.Auth0RoleId,
                roleEntity.Name,
                roleEntity.Description
            );
        }

        public async Task<List<Role>> GetRoles()
        {
            var roleEntities = await locatorDb.Set<RoleEntity>().ToListAsync();
            return roleEntities
                .Select(roleEntity => new Role(
                    roleEntity.RoleId,
                    roleEntity.Auth0RoleId,
                    roleEntity.Name,
                    roleEntity.Description
                ))
                .ToList();
        }

        public async Task UpdateRole(int roleId, string roleName, string roleDescription)
        {
            var role = await locatorDb
                .Set<RoleEntity>()
                .FirstOrDefaultAsync(r => r.RoleId == roleId);

            if (role != null)
            {
                role.Name = roleName;
                role.Description = roleDescription;

                locatorDb.Update(role);
                await locatorDb.SaveChangesAsync();
            }
        }

        public async Task DeleteRole(int roleId)
        {
            var role = await locatorDb
                .Set<RoleEntity>()
                .FirstOrDefaultAsync(r => r.RoleId == roleId);

            if (role != null)
            {
                locatorDb.Remove(role);
                await locatorDb.SaveChangesAsync();
            }
        }
    }
}

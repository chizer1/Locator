using Locator.Db;
using Locator.Domain;
using Microsoft.EntityFrameworkCore;

namespace Locator.Features.RolePermissions
{
    internal class RolePermissionRepository(DbContext locatorDb) : IRolePermissionRepository
    {
        public async Task<int> AddRolePermission(int roleId, int permissionId)
        {
            var rolePermission = new RolePermissionEntity
            {
                RoleId = roleId,
                PermissionId = permissionId,
            };

            locatorDb.Add(rolePermission);
            await locatorDb.SaveChangesAsync();

            return rolePermission.RolePermissionId;
        }

        public async Task<List<Permission>> GetRolePermissions(int roleId)
        {
            return await locatorDb
                .Set<RolePermissionEntity>()
                .Where(rp => rp.RoleId == roleId)
                .Select(rp => rp.Permission)
                .Select(p => new Permission(
                    p.PermissionId,
                    p.PermissionName,
                    p.PermissionDescription
                ))
                .ToListAsync();
        }

        public async Task DeleteRolePermission(int roleId, int permissionId)
        {
            var rolePermission = await locatorDb
                .Set<RolePermissionEntity>()
                .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);

            if (rolePermission != null)
            {
                locatorDb.Remove(rolePermission);
                await locatorDb.SaveChangesAsync();
            }
        }
    }
}

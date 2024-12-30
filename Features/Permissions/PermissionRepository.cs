using Locator.Db;
using Locator.Domain;
using Microsoft.EntityFrameworkCore;

namespace Locator.Features.Permissions
{
    internal class PermissionRepository(DbContext locatorDb) : IPermissionRepository
    {
        public async Task<int> AddPermission(string permissionName, string permissionDescription)
        {
            var permission = new PermissionEntity
            {
                PermissionName = permissionName,
                PermissionDescription = permissionDescription,
            };

            locatorDb.Add(permission);
            await locatorDb.SaveChangesAsync();

            return permission.PermissionId;
        }

        public async Task<Permission> GetPermission(int permissionId)
        {
            var permission = await locatorDb
                .Set<PermissionEntity>()
                .FirstOrDefaultAsync(p => p.PermissionId == permissionId);

            if (permission == null)
                return null;

            return new Permission(
                permission.PermissionId,
                permission.PermissionName,
                permission.PermissionDescription
            );
        }

        public async Task<List<Permission>> GetPermissions()
        {
            var permissions = await locatorDb.Set<PermissionEntity>().ToListAsync();

            return permissions
                .Select(pe => new Permission(
                    pe.PermissionId,
                    pe.PermissionName,
                    pe.PermissionDescription
                ))
                .ToList();
        }

        public async Task UpdatePermission(
            int permissionId,
            string permissionName,
            string permissionDescription
        )
        {
            var permission =
                await locatorDb
                    .Set<PermissionEntity>()
                    .FirstOrDefaultAsync(p => p.PermissionId == permissionId)
                ?? throw new KeyNotFoundException($"Permission with ID {permissionId} not found.");

            permission.PermissionName = permissionName;
            permission.PermissionDescription = permissionDescription;

            locatorDb.Update(permission);
            await locatorDb.SaveChangesAsync();
        }

        public async Task DeletePermission(int permissionId)
        {
            var permission =
                await locatorDb
                    .Set<PermissionEntity>()
                    .FirstOrDefaultAsync(p => p.PermissionId == permissionId)
                ?? throw new KeyNotFoundException($"Permission with ID {permissionId} not found.");

            locatorDb.Remove(permission);
            await locatorDb.SaveChangesAsync();
        }
    }
}

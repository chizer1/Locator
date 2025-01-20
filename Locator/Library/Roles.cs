using Locator.Common;
using Locator.Domain;
using Locator.Features.Roles;
using Locator.Features.Roles.AddRole;
using Locator.Features.Roles.DeleteRole;
using Locator.Features.Roles.GetRoles;
using Locator.Features.Roles.UpdateRole;
using Microsoft.EntityFrameworkCore;

namespace Locator.Library;

internal class Roles
{
    private readonly AddRole _addRoles;
    private readonly GetRoles _getRoles;
    private readonly DeleteRole _deleteRoles;
    private readonly UpdateRole _updateRoles;

    public Roles(DbContext locatorDb, Auth0 auth0)
    {
        IRoleRepository roleRepository = new RoleRepository(locatorDb);
        IAuth0RoleService auth0UserService = new Auth0RoleService(auth0);

        _addRoles = new AddRole(roleRepository, auth0UserService);
        _getRoles = new GetRoles(roleRepository);
        _deleteRoles = new DeleteRole(roleRepository, auth0UserService);
        _updateRoles = new UpdateRole(roleRepository, auth0UserService);
    }

    public async Task<int> AddRole(string roleName, string roleDescription)
    {
        return await _addRoles.Handle(new AddRoleCommand(roleName, roleDescription));
    }

    public async Task<List<Role>> GetRoles()
    {
        return await _getRoles.Handle(new GetRolesQuery());
    }

    public async Task UpdateRole(int roleId, string roleName, string roleDescription)
    {
        await _updateRoles.Handle(new UpdateRoleCommand(roleId, roleName, roleDescription));
    }

    public async Task DeleteRole(int roleId)
    {
        await _deleteRoles.Handle(new DeleteRoleCommand(roleId));
    }
}

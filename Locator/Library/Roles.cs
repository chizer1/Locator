using System.Data.SqlClient;
using Locator.Domain;
using Locator.Features.Roles;
using Locator.Features.Roles.AddRole;
using Locator.Features.Roles.DeleteRole;
using Locator.Features.Roles.GetRoles;
using Locator.Features.Roles.UpdateRole;

namespace Locator.Library;

public class Roles
{
    private readonly AddRole _addRoles;
    private readonly GetRoles _getRoles;
    private readonly DeleteRole _deleteRoles;
    private readonly UpdateRole _updateRoles;

    public Roles(SqlConnection locatorDb)
    {
        IRoleRepository roleRepository = new RoleRepository(locatorDb);

        _addRoles = new AddRole(roleRepository);
        _getRoles = new GetRoles(roleRepository);
        _deleteRoles = new DeleteRole(roleRepository);
        _updateRoles = new UpdateRole(roleRepository);
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

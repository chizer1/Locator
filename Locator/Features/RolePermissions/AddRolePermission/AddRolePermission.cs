using FluentValidation;
using Locator.Features.Roles;

namespace Locator.Features.RolePermissions.AddRolePermission;

internal class AddRolePermissionCommand(int roleId, int permissionId)
{
    public int RoleId => roleId;
    public int PermissionId => permissionId;
}

internal sealed class AddRolePermissionCommandValidator
    : AbstractValidator<AddRolePermissionCommand>
{
    public AddRolePermissionCommandValidator()
    {
        RuleFor(command => command.RoleId).NotEmpty().WithMessage("RoleId is required");
        RuleFor(command => command.PermissionId).NotEmpty().WithMessage("PermissionId is required");
    }
}

internal class AddRolePermission(
    IRolePermissionRepository rolePermissionRepository,
    IRoleRepository roleRepository,
    IAuth0RolePermissionService auth0RolePermissionService
)
{
    public async Task<int> Handle(AddRolePermissionCommand command)
    {
        await new AddRolePermissionCommandValidator().ValidateAndThrowAsync(command);

        var role = await roleRepository.GetRole(command.RoleId);

        var rolePermissionId = await rolePermissionRepository.AddRolePermission(
            command.RoleId,
            command.PermissionId
        );

        var rolePermissions = await rolePermissionRepository.GetRolePermissions(command.RoleId);

        await auth0RolePermissionService.UpdateRolePermissions(role.Auth0RoleId, rolePermissions);

        return rolePermissionId;
    }
}

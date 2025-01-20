using FluentValidation;
using Locator.Features.Roles;

namespace Locator.Features.RolePermissions.DeleteRolePermission;

internal class DeleteRolePermissionCommand(int roleId, int permissionId)
{
    public int RoleId => roleId;
    public int PermissionId => permissionId;
}

internal sealed class DeleteRolePermissionCommandValidator
    : AbstractValidator<DeleteRolePermissionCommand>
{
    public DeleteRolePermissionCommandValidator()
    {
        RuleFor(command => command.RoleId).NotEmpty().WithMessage("ClientId is required");
        RuleFor(command => command.PermissionId).NotEmpty().WithMessage("UserId is required");
    }
}

internal class DeleteRolePermission(
    IRolePermissionRepository rolePermissionRepository,
    IRoleRepository roleRepository,
    IAuth0RolePermissionService auth0RolePermissionService
)
{
    public async Task Handle(DeleteRolePermissionCommand command)
    {
        await new DeleteRolePermissionCommandValidator().ValidateAndThrowAsync(command);

        await rolePermissionRepository.DeleteRolePermission(command.RoleId, command.PermissionId);

        var role = await roleRepository.GetRole(command.RoleId);
        var rolePermissions = await rolePermissionRepository.GetRolePermissions(command.RoleId);

        await auth0RolePermissionService.UpdateRolePermissions(role.Auth0RoleId, rolePermissions);
    }
}

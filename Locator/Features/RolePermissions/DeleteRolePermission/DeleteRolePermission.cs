using FluentValidation;

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

internal class DeleteRolePermission(IRolePermissionRepository rolePermissionRepository)
{
    public async Task Handle(DeleteRolePermissionCommand command)
    {
        await new DeleteRolePermissionCommandValidator().ValidateAndThrowAsync(command);

        await rolePermissionRepository.DeleteRolePermission(command.RoleId, command.PermissionId);
    }
}

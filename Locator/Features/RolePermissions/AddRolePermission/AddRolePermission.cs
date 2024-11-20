using FluentValidation;

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

internal class AddRolePermission(IRolePermissionRepository rolePermissionRepository)
{
    public async Task<int> Handle(AddRolePermissionCommand command)
    {
        await new AddRolePermissionCommandValidator().ValidateAndThrowAsync(command);

        return await rolePermissionRepository.AddRolePermission(
            command.RoleId,
            command.PermissionId
        );
    }
}

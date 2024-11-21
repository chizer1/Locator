using FluentValidation;

namespace Locator.Features.Permissions.DeletePermission;

internal class DeletePermissionCommand(int permissionId)
{
    public int PermissionId => permissionId;
}

internal sealed class DeletePermissionCommandValidator : AbstractValidator<DeletePermissionCommand>
{
    public DeletePermissionCommandValidator()
    {
        RuleFor(x => x.PermissionId).NotEmpty().WithMessage("Id is required.");
    }
}

internal class DeletePermission(
    IPermissionRepository permissionRepository,
    IAuth0PermissionService auth0PermissionService
)
{
    public async Task Handle(DeletePermissionCommand command)
    {
        await new DeletePermissionCommandValidator().ValidateAndThrowAsync(command);

        await permissionRepository.DeletePermission(command.PermissionId);

        var permissions = await permissionRepository.GetPermissions();

        await auth0PermissionService.UpdatePermissions(permissions);
    }
}

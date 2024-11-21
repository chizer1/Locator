using FluentValidation;

namespace Locator.Features.Permissions.UpdatePermission;

internal class UpdatePermissionCommand(
    int permissionId,
    string permissionName,
    string permissionDescription
)
{
    public int PermissionId => permissionId;
    public string PermissionName => permissionName;
    public string PermissionDescription => permissionDescription;
}

internal sealed class UpdatePermissionCommandValidator : AbstractValidator<UpdatePermissionCommand>
{
    public UpdatePermissionCommandValidator()
    {
        RuleFor(x => x.PermissionId).NotNull().WithMessage("Id cannot be null");
        RuleFor(x => x.PermissionName).NotEmpty().WithMessage("Permission Name is required.");
        RuleFor(x => x.PermissionDescription)
            .NotEmpty()
            .WithMessage("Permission Description is required.");
    }
}

internal class UpdatePermission(
    IPermissionRepository permissionRepository,
    IAuth0PermissionService auth0PermissionService
)
{
    public async Task Handle(UpdatePermissionCommand command)
    {
        await new UpdatePermissionCommandValidator().ValidateAndThrowAsync(command);

        await permissionRepository.UpdatePermission(
            command.PermissionId,
            command.PermissionName,
            command.PermissionDescription
        );

        var permissions = await permissionRepository.GetPermissions();

        await auth0PermissionService.UpdatePermissions(permissions);
    }
}

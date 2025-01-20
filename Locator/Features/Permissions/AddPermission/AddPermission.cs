using FluentValidation;
using Locator.Domain;

namespace Locator.Features.Permissions.AddPermission;

internal class AddPermissionCommand(string permissionName, string permissionDescription)
{
    public string PermissionName => permissionName;
    public string PermissionDescription => permissionDescription;
}

internal sealed class AddPermissionCommandValidator : AbstractValidator<AddPermissionCommand>
{
    public AddPermissionCommandValidator()
    {
        RuleFor(x => x.PermissionName).NotEmpty().WithMessage("Permission Name is required.");
        RuleFor(x => x.PermissionDescription)
            .NotEmpty()
            .WithMessage("Permission Description is required.");
    }
}

internal class AddPermission(
    IPermissionRepository permissionRepository,
    IAuth0PermissionService auth0PermissionService
)
{
    public async Task<int> Handle(AddPermissionCommand command)
    {
        await new AddPermissionCommandValidator().ValidateAndThrowAsync(command);

        var permissions = await permissionRepository.GetPermissions();
        permissions.Add(
            new Permission(
                id: 0,
                name: command.PermissionName,
                description: command.PermissionDescription
            )
        );
        await auth0PermissionService.UpdatePermissions(permissions);

        return await permissionRepository.AddPermission(
            command.PermissionName,
            command.PermissionDescription
        );
    }
}

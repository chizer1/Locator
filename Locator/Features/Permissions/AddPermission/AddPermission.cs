using FluentValidation;

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

internal class AddPermission(IPermissionRepository permissionRepository)
{
    public async Task<int> Handle(AddPermissionCommand command)
    {
        await new AddPermissionCommandValidator().ValidateAndThrowAsync(command);

        return await permissionRepository.AddPermission(
            command.PermissionName,
            command.PermissionDescription
        );
    }
}

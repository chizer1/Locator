using FluentValidation;

namespace Locator.Features.Roles.UpdateRole;

internal class UpdateRoleCommand(int roleId, string roleName, string roleDescription)
{
    public int RoleId => roleId;
    public string RoleName => roleName;
    public string RoleDescription => roleDescription;
}

internal sealed class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator()
    {
        RuleFor(x => x.RoleId).NotNull().WithMessage("Id cannot be null");
        RuleFor(x => x.RoleName).NotEmpty().WithMessage("Role Name is required.");
        RuleFor(x => x.RoleDescription).NotEmpty().WithMessage("Role Description is required.");
    }
}

internal class UpdateRole(IRoleRepository roleRepository, IAuth0RoleService auth0RoleService)
{
    public async Task Handle(UpdateRoleCommand command)
    {
        await new UpdateRoleCommandValidator().ValidateAndThrowAsync(command);

        var role = await roleRepository.GetRole(command.RoleId);

        await auth0RoleService.UpdateRole(
            role.Auth0RoleId,
            command.RoleName,
            command.RoleDescription
        );
        await roleRepository.UpdateRole(command.RoleId, command.RoleName, command.RoleDescription);
    }
}

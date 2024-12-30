using FluentValidation;

namespace Locator.Features.Roles.AddRole;

internal class AddRoleCommand(string roleName, string roleDescription)
{
    public string RoleName => roleName;
    public string RoleDescription => roleDescription;
}

internal sealed class AddRoleCommandValidator : AbstractValidator<AddRoleCommand>
{
    public AddRoleCommandValidator()
    {
        RuleFor(x => x.RoleName).NotEmpty().WithMessage("Role Name is required.");
        RuleFor(x => x.RoleDescription).NotEmpty().WithMessage("Role Description is required.");
    }
}

internal class AddRole(IRoleRepository roleRepository, IAuth0RoleService auth0RoleService)
{
    public async Task<int> Handle(AddRoleCommand command)
    {
        await new AddRoleCommandValidator().ValidateAndThrowAsync(command);

        var auth0RoleId = await auth0RoleService.AddRole(command.RoleName, command.RoleDescription);

        return await roleRepository.AddRole(auth0RoleId, command.RoleName, command.RoleDescription);
    }
}

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

internal class AddRole(IRoleRepository roleRepository)
{
    public async Task<int> Handle(AddRoleCommand command)
    {
        await new AddRoleCommandValidator().ValidateAndThrowAsync(command);

        return await roleRepository.AddRole(
            "Auth0RoleIdHere",
            command.RoleName,
            command.RoleDescription
        );
    }
}

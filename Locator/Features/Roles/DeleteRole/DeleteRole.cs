using FluentValidation;

namespace Locator.Features.Roles.DeleteRole;

internal class DeleteRoleCommand(int roleId)
{
    public int RoleId => roleId;
}

internal sealed class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
{
    public DeleteRoleCommandValidator()
    {
        RuleFor(x => x.RoleId).NotEmpty().WithMessage("Id is required.");
    }
}

internal class DeleteRole(IRoleRepository roleRepository, IAuth0RoleService auth0RoleService)
{
    public async Task Handle(DeleteRoleCommand command)
    {
        await new DeleteRoleCommandValidator().ValidateAndThrowAsync(command);

        var role = await roleRepository.GetRole(command.RoleId);
        await auth0RoleService.DeleteRole(role.Auth0RoleId);

        await roleRepository.DeleteRole(command.RoleId);
    }
}

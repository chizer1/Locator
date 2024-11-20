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

internal class DeleteRole(IRoleRepository roleRepository)
{
    public async Task Handle(DeleteRoleCommand command)
    {
        await new DeleteRoleCommandValidator().ValidateAndThrowAsync(command);

        await roleRepository.DeleteRole(command.RoleId);
    }
}

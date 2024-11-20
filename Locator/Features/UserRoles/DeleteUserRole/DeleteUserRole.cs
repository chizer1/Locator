using FluentValidation;

namespace Locator.Features.UserRoles.DeleteUserRole;

internal class DeleteUserRoleCommand(int userId, int roleId)
{
    public int UserId => userId;
    public int RoleId => roleId;
}

internal sealed class DeleteUserRoleCommandValidator : AbstractValidator<DeleteUserRoleCommand>
{
    public DeleteUserRoleCommandValidator()
    {
        RuleFor(command => command.UserId).NotEmpty().WithMessage("UserId is required");
        RuleFor(command => command.RoleId).NotEmpty().WithMessage("RoleId is required");
    }
}

internal class DeleteUserRole(IUserRoleRepository userRoleRepository)
{
    public async Task Handle(DeleteUserRoleCommand command)
    {
        await new DeleteUserRoleCommandValidator().ValidateAndThrowAsync(command);

        await userRoleRepository.DeleteUserRole(command.UserId, command.RoleId);
    }
}

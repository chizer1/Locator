using FluentValidation;
using Locator.Features.Roles;
using Locator.Features.Users;

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

internal class DeleteUserRole(
    IUserRoleRepository userRoleRepository,
    IRoleRepository roleRepository,
    IUserRepository userRepository,
    IAuth0UserRoleService auth0UserRoleService
)
{
    public async Task Handle(DeleteUserRoleCommand command)
    {
        await new DeleteUserRoleCommandValidator().ValidateAndThrowAsync(command);

        var user = await userRepository.GetUser(command.UserId);
        var role = await roleRepository.GetRole(command.RoleId);

        await auth0UserRoleService.DeleteUserRole(user.Auth0Id, role.Auth0RoleId);

        await userRoleRepository.DeleteUserRole(command.UserId, command.RoleId);
    }
}

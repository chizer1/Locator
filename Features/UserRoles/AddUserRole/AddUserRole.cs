using FluentValidation;
using Locator.Features.Roles;
using Locator.Features.Users;

namespace Locator.Features.UserRoles.AddUserRole;

internal class AddUserRoleCommand(int userId, int roleId)
{
    public int UserId => userId;
    public int RoleId => roleId;
}

internal sealed class AddUserRoleCommandValidator : AbstractValidator<AddUserRoleCommand>
{
    public AddUserRoleCommandValidator()
    {
        RuleFor(command => command.UserId).NotEmpty().WithMessage("UserId is required");
        RuleFor(command => command.RoleId).NotEmpty().WithMessage("RoleId is required");
    }
}

internal class AddUserRole(
    IUserRoleRepository userRoleRepository,
    IRoleRepository roleRepository,
    IUserRepository userRepository,
    IAuth0UserRoleService auth0UserRoleService
)
{
    public async Task<int> Handle(AddUserRoleCommand command)
    {
        await new AddUserRoleCommandValidator().ValidateAndThrowAsync(command);

        var user = await userRepository.GetUser(command.UserId);
        var role = await roleRepository.GetRole(command.RoleId);

        await auth0UserRoleService.AddUserRole(user.Auth0Id, role.Auth0RoleId);

        return await userRoleRepository.AddUserRole(command.UserId, command.RoleId);
    }
}

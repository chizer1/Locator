using FluentValidation;

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

internal class AddUserRole(IUserRoleRepository userRoleRepository)
{
    public async Task<int> Handle(AddUserRoleCommand command)
    {
        await new AddUserRoleCommandValidator().ValidateAndThrowAsync(command);

        return await userRoleRepository.AddUserRole(command.UserId, command.RoleId);
    }
}

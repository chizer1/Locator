using FluentValidation;

namespace Locator.Features.Users.DeleteUser;

internal class DeleteUserCommand(int userId)
{
    public int UserId { get; } = userId;
}

internal sealed class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("Id cannot be empty");
    }
}

internal class DeleteUser(IUserRepository userRepository, IAuth0UserService auth0UserService)
{
    public async Task Handle(DeleteUserCommand command)
    {
        await new DeleteUserCommandValidator().ValidateAndThrowAsync(command);

        var user = await userRepository.GetUser(command.UserId);
        if (user == null)
            throw new ValidationException($"This user doesn't exist.");

        await auth0UserService.DeleteUser(user.Auth0Id);

        await userRepository.DeleteUser(command.UserId);
    }
}

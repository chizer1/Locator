using FluentValidation;
using Locator.Domain;

namespace Locator.Features.Users.UpdateUser;

internal class UpdateUserCommand(
    int userId,
    string firstName,
    string lastName,
    string emailAddress,
    string password,
    UserStatus userStatus
)
{
    public int UserId => userId;
    public string FirstName => firstName;
    public string LastName => lastName;
    public string EmailAddress => emailAddress;
    public string Password => password;
    public UserStatus UserStatus => userStatus;
}

internal sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("Id cannot be empty");
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required.");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required.");
        RuleFor(x => x.EmailAddress)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("A valid email is required.");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");
        RuleFor(x => x.UserStatus).NotEmpty().WithMessage("UserStatus is required.");
    }
}

internal class UpdateUser(IUserRepository userRepository, IAuth0UserService auth0UserService)
{
    public async Task Handle(UpdateUserCommand command)
    {
        await new UpdateUserCommandValidator().ValidateAndThrowAsync(command);

        var user = await userRepository.GetUser(command.UserId);
        if (user == null)
            throw new ValidationException($"This user doesn't exist.");

        await auth0UserService.UpdateUser(
            user.Auth0Id,
            command.FirstName,
            command.LastName,
            command.EmailAddress,
            command.UserStatus
        );

        await userRepository.UpdateUser(
            command.UserId,
            command.FirstName,
            command.LastName,
            command.EmailAddress,
            command.UserStatus
        );
    }
}

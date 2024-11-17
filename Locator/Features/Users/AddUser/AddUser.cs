using FluentValidation;
using Locator.Domain;

namespace Locator.Features.Users.AddUser;

internal class AddUserCommand(
    string firstName,
    string lastName,
    string emailAddress,
    string password,
    UserStatus userStatus
)
{
    public string FirstName { get; } = firstName;
    public string LastName { get; } = lastName;
    public string EmailAddress { get; } = emailAddress;
    public string Password { get; } = password;
    public UserStatus UserStatus { get; } = userStatus;
}

internal sealed class AddUserCommandValidator : AbstractValidator<AddUserCommand>
{
    public AddUserCommandValidator()
    {
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

internal class AddUser(IUserRepository userRepository, IAuth0UserService auth0UserService)
{
    public async Task<int> Handle(AddUserCommand command)
    {
        await new AddUserCommandValidator().ValidateAndThrowAsync(command);

        var user = await userRepository.GetUser(command.EmailAddress);
        if (user != null)
            throw new ValidationException($"{command.EmailAddress} already exists.");

        var auth0Id = await auth0UserService.AddUser(
            command.FirstName,
            command.LastName,
            command.EmailAddress,
            command.Password,
            command.UserStatus
        );

        return await userRepository.AddUser(
            command.FirstName,
            command.LastName,
            command.EmailAddress,
            command.UserStatus,
            auth0Id
        );
    }
}

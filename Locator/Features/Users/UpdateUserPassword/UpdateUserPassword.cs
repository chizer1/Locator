using FluentValidation;

namespace Locator.Features.Users.UpdateUserPassword;

internal class UpdateUserPasswordCommand(string auth0Id, string password)
{
    public string Auth0Id => auth0Id;
    public string Password => password;
}

internal sealed class UpdateUserPasswordCommandValidator
    : AbstractValidator<UpdateUserPasswordCommand>
{
    public UpdateUserPasswordCommandValidator()
    {
        RuleFor(x => x.Auth0Id).NotEmpty().WithMessage("Id cannot be empty");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");
    }
}

internal class UpdateUserPassword(IAuth0UserService auth0UserService)
{
    public async Task Handle(UpdateUserPasswordCommand command)
    {
        await new UpdateUserPasswordCommandValidator().ValidateAndThrowAsync(command);

        await auth0UserService.UpdateUserPassword(command.Auth0Id, command.Password);
    }
}

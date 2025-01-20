using FluentValidation;

namespace Locator.Features.Users.GeneratePasswordChangeTicket;

internal class GeneratePasswordChangeTicketCommand(string auth0Id, string redirectUrl)
{
    public string Auth0Id => auth0Id;
    public string RedirectUrl => redirectUrl;
}

internal sealed class GeneratePasswordChangeTicketCommandValidator
    : AbstractValidator<GeneratePasswordChangeTicketCommand>
{
    public GeneratePasswordChangeTicketCommandValidator()
    {
        RuleFor(x => x.Auth0Id).NotEmpty().WithMessage("Id cannot be empty");
        RuleFor(x => x.RedirectUrl).NotEmpty().WithMessage("Redirect Url is required.");
    }
}

internal class GeneratePasswordChangeTicket(IAuth0UserService auth0UserService)
{
    public async Task<string> Handle(GeneratePasswordChangeTicketCommand command)
    {
        await new GeneratePasswordChangeTicketCommandValidator().ValidateAndThrowAsync(command);

        return await auth0UserService.GeneratePasswordChangeTicket(
            command.Auth0Id,
            command.RedirectUrl
        );
    }
}

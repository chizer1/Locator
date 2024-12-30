using FluentValidation;

namespace Locator.Features.ClientUsers.AddClientUser;

internal class AddClientUserCommand(int clientId, int userId)
{
    public int ClientId => clientId;
    public int UserId => userId;
}

internal sealed class AddClientUserCommandValidator : AbstractValidator<AddClientUserCommand>
{
    public AddClientUserCommandValidator()
    {
        RuleFor(command => command.ClientId).NotEmpty().WithMessage("ClientId is required");
        RuleFor(command => command.UserId).NotEmpty().WithMessage("UserId is required");
    }
}

internal class AddClientUser(IClientUserRepository clientUserRepository)
{
    public async Task<int> Handle(AddClientUserCommand command)
    {
        await new AddClientUserCommandValidator().ValidateAndThrowAsync(command);

        return await clientUserRepository.AddClientUser(command.ClientId, command.UserId);
    }
}

using FluentValidation;

namespace Locator.Features.ClientUsers.DeleteClientUser;

internal class DeleteClientUserCommand(int clientId, int userId)
{
    public int ClientId => clientId;
    public int UserId => userId;
}

internal sealed class DeleteClientUserCommandValidator : AbstractValidator<DeleteClientUserCommand>
{
    public DeleteClientUserCommandValidator()
    {
        RuleFor(command => command.ClientId).NotEmpty().WithMessage("ClientId is required");
        RuleFor(command => command.UserId).NotEmpty().WithMessage("UserId is required");
    }
}

internal class DeleteClientUser(IClientUserRepository clientUserRepository)
{
    public async Task Handle(DeleteClientUserCommand command)
    {
        await new DeleteClientUserCommandValidator().ValidateAndThrowAsync(command);

        await clientUserRepository.DeleteClientUser(command.ClientId, command.UserId);
    }
}

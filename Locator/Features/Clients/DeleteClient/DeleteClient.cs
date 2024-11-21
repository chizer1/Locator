using FluentValidation;

namespace Locator.Features.Clients.DeleteClient;

internal class DeleteClientCommand(int clientId)
{
    public int ClientId => clientId;
}

internal sealed class DeleteClientCommandValidator : AbstractValidator<DeleteClientCommand>
{
    public DeleteClientCommandValidator()
    {
        RuleFor(x => x.ClientId).NotEmpty().WithMessage("Id is required.");
    }
}

internal class DeleteClient(IClientRepository clientRepository)
{
    public async Task Handle(DeleteClientCommand command)
    {
        await new DeleteClientCommandValidator().ValidateAndThrowAsync(command);

        await clientRepository.DeleteClient(command.ClientId);
    }
}

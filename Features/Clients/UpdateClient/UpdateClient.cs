using FluentValidation;
using Locator.Domain;

namespace Locator.Features.Clients.UpdateClient;

internal class UpdateClientCommand(
    int clientId,
    string clientName,
    string clientCode,
    Status clientStatus
)
{
    public int ClientId => clientId;
    public string ClientName => clientName;
    public string ClientCode => clientCode;
    public Status ClientStatus => clientStatus;
}

internal sealed class UpdateClientCommandValidator : AbstractValidator<UpdateClientCommand>
{
    public UpdateClientCommandValidator()
    {
        RuleFor(x => x.ClientId).NotNull().WithMessage("Id cannot be null");
        RuleFor(x => x.ClientName).NotEmpty().WithMessage("Client Name is required.");
        RuleFor(x => x.ClientCode).NotEmpty().WithMessage("Client Code is required.");
        RuleFor(x => x.ClientStatus).NotEmpty().WithMessage("Client Status is required.");
    }
}

internal class UpdateClient(IClientRepository clientRepository)
{
    public async Task Handle(UpdateClientCommand command)
    {
        await new UpdateClientCommandValidator().ValidateAndThrowAsync(command);

        await clientRepository.UpdateClient(
            command.ClientId,
            command.ClientName,
            command.ClientCode,
            command.ClientStatus
        );
    }
}

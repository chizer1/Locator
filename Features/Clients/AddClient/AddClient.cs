using FluentValidation;
using Locator.Domain;

namespace Locator.Features.Clients.AddClient;

internal class AddClientCommand(string clientName, string clientCode, Status clientStatus)
{
    public string ClientName => clientName;
    public string ClientCode => clientCode;
    public Status ClientStatus => clientStatus;
}

internal sealed class AddClientCommandValidator : AbstractValidator<AddClientCommand>
{
    public AddClientCommandValidator()
    {
        RuleFor(x => x.ClientName).NotEmpty().WithMessage("Client Name is required.");
        RuleFor(x => x.ClientCode).NotEmpty().WithMessage("Client Code is required.");
        RuleFor(x => x.ClientStatus).NotEmpty().WithMessage("Client Status is required.");
    }
}

internal class AddClient(IClientRepository clientRepository)
{
    public async Task<int> Handle(AddClientCommand command)
    {
        await new AddClientCommandValidator().ValidateAndThrowAsync(command);

        return await clientRepository.AddClient(
            command.ClientName,
            command.ClientCode,
            command.ClientStatus
        );
    }
}

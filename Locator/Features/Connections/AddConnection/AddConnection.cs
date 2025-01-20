using FluentValidation;

namespace Locator.Features.Connections.AddConnection;

internal class AddConnectionCommand(int clientUserId, int databaseId)
{
    public int ClientUserId => clientUserId;
    public int DatabaseId => databaseId;
}

internal sealed class AddConnectionCommandValidator : AbstractValidator<AddConnectionCommand>
{
    public AddConnectionCommandValidator()
    {
        RuleFor(command => command.ClientUserId).NotEmpty().WithMessage("ClientUserId is required");
        RuleFor(command => command.DatabaseId).NotEmpty().WithMessage("DatabaseId is required");
    }
}

internal class AddConnection(IConnectionRepository connectionRepository)
{
    public async Task<int> Handle(AddConnectionCommand command)
    {
        await new AddConnectionCommandValidator().ValidateAndThrowAsync(command);

        return await connectionRepository.AddConnection(command.ClientUserId, command.DatabaseId);
    }
}

using FluentValidation;

namespace Locator.Features.Connections.DeleteConnection;

internal class DeleteConnectionCommand(int connectionId)
{
    public int ConnectionId => connectionId;
}

internal sealed class DeleteConnectionCommandValidator : AbstractValidator<DeleteConnectionCommand>
{
    public DeleteConnectionCommandValidator()
    {
        RuleFor(command => command.ConnectionId).NotEmpty().WithMessage("ConnectionId is required");
    }
}

internal class DeleteConnection(IConnectionRepository connectionRepository)
{
    public async Task Handle(DeleteConnectionCommand command)
    {
        await new DeleteConnectionCommandValidator().ValidateAndThrowAsync(command);

        await connectionRepository.DeleteConnection(command.ConnectionId);
    }
}

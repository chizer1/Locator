using FluentValidation;

namespace Locator.Features.DatabaseServers.DeleteDatabaseServer;

internal class DeleteDatabaseServerCommand(int databaseServerId)
{
    public int DatabaseServerId => databaseServerId;
}

internal sealed class DeleteDatabaseServerCommandValidator
    : AbstractValidator<DeleteDatabaseServerCommand>
{
    public DeleteDatabaseServerCommandValidator()
    {
        RuleFor(x => x.DatabaseServerId).NotEmpty().WithMessage("Id is required.");
    }
}

internal class DeleteDatabaseServer(IDatabaseServerRepository databaseServerRepository)
{
    public async Task Handle(DeleteDatabaseServerCommand command)
    {
        await new DeleteDatabaseServerCommandValidator().ValidateAndThrowAsync(command);

        await databaseServerRepository.DeleteDatabaseServer(command.DatabaseServerId);
    }
}

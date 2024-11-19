using FluentValidation;
using Locator.Domain;

namespace Locator.Features.DatabaseServers.UpdateDatabaseServer;

internal class UpdateDatabaseServerCommand(
    int databaseServerId,
    string databaseServerName,
    string databaseServerIpAddress
)
{
    public int DatabaseServerId => databaseServerId;
    public string DatabaseServerName => databaseServerName;
    public string DatabaseServerIpAddress => databaseServerIpAddress;
}

internal sealed class UpdateDatabaseServerCommandValidator
    : AbstractValidator<UpdateDatabaseServerCommand>
{
    public UpdateDatabaseServerCommandValidator()
    {
        RuleFor(x => x.DatabaseServerId).NotNull().WithMessage("Id cannot be null");
        RuleFor(x => x.DatabaseServerName)
            .NotEmpty()
            .WithMessage("DatabaseServer Name is required.");
        RuleFor(x => x.DatabaseServerIpAddress)
            .NotEmpty()
            .WithMessage("Database Server IP Address is required.");
    }
}

internal class UpdateDatabaseServer(IDatabaseServerRepository databaseServerRepository)
{
    public async Task Handle(UpdateDatabaseServerCommand command)
    {
        await new UpdateDatabaseServerCommandValidator().ValidateAndThrowAsync(command);

        await databaseServerRepository.UpdateDatabaseServer(
            command.DatabaseServerId,
            command.DatabaseServerName,
            command.DatabaseServerIpAddress
        );
    }
}

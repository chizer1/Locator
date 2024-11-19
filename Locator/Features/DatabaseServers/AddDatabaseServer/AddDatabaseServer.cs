using FluentValidation;

namespace Locator.Features.DatabaseServers.AddDatabaseServer;

internal class AddDatabaseServerCommand(string databaseServerName, string databaseServerIpAddress)
{
    public string DatabaseServerName => databaseServerName;
    public string DatabaseServerIpAddress => databaseServerIpAddress;
}

internal sealed class AddDatabaseServerCommandValidator
    : AbstractValidator<AddDatabaseServerCommand>
{
    public AddDatabaseServerCommandValidator()
    {
        RuleFor(x => x.DatabaseServerName)
            .NotEmpty()
            .WithMessage("DatabaseServer Name is required.");
        RuleFor(x => x.DatabaseServerIpAddress)
            .NotEmpty()
            .WithMessage("Database Server IP Address is required.");
    }
}

internal class AddDatabaseServer(IDatabaseServerRepository databaseServerRepository)
{
    public async Task<int> Handle(AddDatabaseServerCommand command)
    {
        await new AddDatabaseServerCommandValidator().ValidateAndThrowAsync(command);

        return await databaseServerRepository.AddDatabaseServer(
            command.DatabaseServerName,
            command.DatabaseServerIpAddress
        );
    }
}

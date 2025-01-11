using FluentValidation;
using Locator.Domain;

namespace Locator.Features.Databases.AddDatabase;

internal class AddDatabaseCommand(
    string databaseName,
    string databaseUser,
    int databaseServerId,
    byte databaseTypeId,
    Status databaseStatus,
    bool useTrustedConnection,
    bool createDatabase
)
{
    public string DatabaseName => databaseName;
    public string DatabaseUser => databaseUser;
    public int DatabaseServerId => databaseServerId;
    public byte DatabaseTypeId => databaseTypeId;
    public Status DatabaseStatus => databaseStatus;
    public bool UseTrustedConnection => useTrustedConnection;
    public bool CreateDatabase => createDatabase;
}

internal sealed class AddDatabaseCommandValidator : AbstractValidator<AddDatabaseCommand>
{
    public AddDatabaseCommandValidator()
    {
        RuleFor(x => x.DatabaseName).NotEmpty().WithMessage("Database Name is required.");
        RuleFor(x => x.DatabaseUser).NotEmpty().WithMessage("Database User is required.");
        RuleFor(x => x.DatabaseServerId).NotEmpty().WithMessage("Database Server is required.");
        RuleFor(x => x.DatabaseTypeId).NotEmpty().WithMessage("Database Type is required.");
        RuleFor(x => x.DatabaseStatus).NotEmpty().WithMessage("Database Status is required.");
        // RuleFor(x => x.UseTrustedConnection)
        //     .NotEmpty()
        //     .WithMessage("Use Trusted Connection is required.");
        //RuleFor(x => x.CreateDatabase).NotEmpty().WithMessage("Create Database is required.");
    }
}

internal class AddDatabase(IDatabaseRepository databaseRepository)
{
    public async Task<int> Handle(AddDatabaseCommand command)
    {
        await new AddDatabaseCommandValidator().ValidateAndThrowAsync(command);

        return await databaseRepository.AddDatabase(
            command.DatabaseName,
            command.DatabaseUser,
            command.DatabaseServerId,
            command.DatabaseTypeId,
            command.DatabaseStatus,
            command.UseTrustedConnection,
            command.CreateDatabase
        );
    }
}

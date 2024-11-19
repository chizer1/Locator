using FluentValidation;
using Locator.Domain;

namespace Locator.Features.Databases.AddDatabase;

internal class AddDatabaseCommand(
    string databaseName,
    string databaseUser,
    int databaseServerId,
    int databaseTypeId,
    DatabaseStatus databaseStatus
)
{
    public string DatabaseName => databaseName;
    public string DatabaseUser => databaseUser;
    public int DatabaseServerId => databaseServerId;
    public int DatabaseTypeId => databaseTypeId;
    public DatabaseStatus DatabaseStatus => databaseStatus;
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
            command.DatabaseStatus
        );
    }
}

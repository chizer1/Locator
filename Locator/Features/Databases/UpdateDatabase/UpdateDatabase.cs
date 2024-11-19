using FluentValidation;
using Locator.Domain;

namespace Locator.Features.Databases.UpdateDatabase;

internal class UpdateDatabaseCommand(
    int databaseId,
    string databaseName,
    string databaseUserName,
    int databaseServerId,
    int databaseTypeId,
    DatabaseStatus databaseStatus
)
{
    public int DatabaseId => databaseId;
    public string DatabaseName => databaseName;
    public string DatabaseUsername => databaseUserName;
    public int DatabaseServerId => databaseServerId;
    public int DatabaseTypeId => databaseTypeId;
    public DatabaseStatus DatabaseStatus => databaseStatus;
}

internal sealed class UpdateDatabaseCommandValidator : AbstractValidator<UpdateDatabaseCommand>
{
    public UpdateDatabaseCommandValidator()
    {
        RuleFor(x => x.DatabaseId).NotNull().WithMessage("Id cannot be null");
        RuleFor(x => x.DatabaseName).NotEmpty().WithMessage("Database Name is required.");
        RuleFor(x => x.DatabaseUsername).NotEmpty().WithMessage("Database Username is required.");
        RuleFor(x => x.DatabaseServerId).NotNull().WithMessage("Database Server is required.");
        RuleFor(x => x.DatabaseTypeId).NotNull().WithMessage("Database Type is required.");
    }
}

internal class UpdateDatabase(IDatabaseRepository databaseRepository)
{
    public async Task Handle(UpdateDatabaseCommand command)
    {
        await new UpdateDatabaseCommandValidator().ValidateAndThrowAsync(command);

        await databaseRepository.UpdateDatabase(
            command.DatabaseId,
            command.DatabaseName,
            command.DatabaseUsername,
            command.DatabaseServerId,
            command.DatabaseTypeId,
            command.DatabaseStatus
        );
    }
}

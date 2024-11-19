using FluentValidation;
using Locator.Domain;

namespace Locator.Features.DatabaseTypes.UpdateDatabaseType;

internal class UpdateDatabaseTypeCommand(int databaseTypeId, string databaseTypeName)
{
    public int DatabaseTypeId => databaseTypeId;
    public string DatabaseTypeName => databaseTypeName;
}

internal sealed class UpdateDatabaseTypeCommandValidator
    : AbstractValidator<UpdateDatabaseTypeCommand>
{
    public UpdateDatabaseTypeCommandValidator()
    {
        RuleFor(x => x.DatabaseTypeId).NotNull().WithMessage("Id cannot be null");
        RuleFor(x => x.DatabaseTypeName).NotEmpty().WithMessage("DatabaseType Name is required.");
    }
}

internal class UpdateDatabaseType(IDatabaseTypeRepository databaseTypeRepository)
{
    public async Task Handle(UpdateDatabaseTypeCommand command)
    {
        await new UpdateDatabaseTypeCommandValidator().ValidateAndThrowAsync(command);

        await databaseTypeRepository.UpdateDatabaseType(
            command.DatabaseTypeId,
            command.DatabaseTypeName
        );
    }
}

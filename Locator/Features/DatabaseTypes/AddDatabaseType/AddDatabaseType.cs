using FluentValidation;

namespace Locator.Features.DatabaseTypes.AddDatabaseType;

internal class AddDatabaseTypeCommand(string databaseTypeName)
{
    public string DatabaseTypeName => databaseTypeName;
}

internal sealed class AddDatabaseTypeCommandValidator : AbstractValidator<AddDatabaseTypeCommand>
{
    public AddDatabaseTypeCommandValidator()
    {
        RuleFor(x => x.DatabaseTypeName).NotEmpty().WithMessage("DatabaseType Name is required.");
    }
}

internal class AddDatabaseType(IDatabaseTypeRepository databaseTypeRepository)
{
    public async Task<int> Handle(AddDatabaseTypeCommand command)
    {
        await new AddDatabaseTypeCommandValidator().ValidateAndThrowAsync(command);

        return await databaseTypeRepository.AddDatabaseType(command.DatabaseTypeName);
    }
}

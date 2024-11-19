using FluentValidation;

namespace Locator.Features.DatabaseTypes.DeleteDatabaseType;

internal class DeleteDatabaseTypeCommand(int databaseTypeId)
{
    public int DatabaseTypeId => databaseTypeId;
}

internal sealed class DeleteDatabaseTypeCommandValidator
    : AbstractValidator<DeleteDatabaseTypeCommand>
{
    public DeleteDatabaseTypeCommandValidator()
    {
        RuleFor(x => x.DatabaseTypeId).NotEmpty().WithMessage("Id is required.");
    }
}

internal class DeleteDatabaseType(IDatabaseTypeRepository databaseTypeRepository)
{
    public async Task Handle(DeleteDatabaseTypeCommand command)
    {
        await new DeleteDatabaseTypeCommandValidator().ValidateAndThrowAsync(command);

        await databaseTypeRepository.DeleteDatabaseType(command.DatabaseTypeId);
    }
}

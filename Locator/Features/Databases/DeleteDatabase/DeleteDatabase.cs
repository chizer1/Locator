using FluentValidation;

namespace Locator.Features.Databases.DeleteDatabase;

internal class DeleteDatabaseCommand(int databaseId)
{
    public int DatabaseId => databaseId;
}

internal sealed class DeleteDatabaseCommandValidator : AbstractValidator<DeleteDatabaseCommand>
{
    public DeleteDatabaseCommandValidator()
    {
        RuleFor(x => x.DatabaseId).NotEmpty().WithMessage("Id is required.");
    }
}

internal class DeleteDatabase(IDatabaseRepository databaseRepository)
{
    public async Task Handle(DeleteDatabaseCommand command)
    {
        await new DeleteDatabaseCommandValidator().ValidateAndThrowAsync(command);

        await databaseRepository.DeleteDatabase(command.DatabaseId);
    }
}

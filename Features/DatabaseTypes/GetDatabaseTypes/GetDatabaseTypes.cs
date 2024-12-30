using FluentValidation;
using Locator.Domain;

namespace Locator.Features.DatabaseTypes.GetDatabaseTypes;

internal class GetDatabaseTypesQuery() { }

internal sealed class GetDatabaseTypesQueryValidator : AbstractValidator<GetDatabaseTypesQuery>
{
    public GetDatabaseTypesQueryValidator() { }
}

internal class GetDatabaseTypes(IDatabaseTypeRepository databaseTypeRepository)
{
    public async Task<List<DatabaseType>> Handle(GetDatabaseTypesQuery query)
    {
        await new GetDatabaseTypesQueryValidator().ValidateAndThrowAsync(query);

        return await databaseTypeRepository.GetDatabaseTypes();
    }
}

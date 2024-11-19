using FluentValidation;
using Locator.Domain;

namespace Locator.Features.DatabaseServers.GetDatabaseServers;

internal class GetDatabaseServersQuery() { }

internal sealed class GetDatabaseServersQueryValidator : AbstractValidator<GetDatabaseServersQuery>
{
    public GetDatabaseServersQueryValidator() { }
}

internal class GetDatabaseServers(IDatabaseServerRepository databaseServerRepository)
{
    public async Task<List<DatabaseServer>> Handle(GetDatabaseServersQuery query)
    {
        await new GetDatabaseServersQueryValidator().ValidateAndThrowAsync(query);

        return await databaseServerRepository.GetDatabaseServers();
    }
}

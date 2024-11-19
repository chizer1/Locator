using System.Data.SqlClient;
using FluentValidation;

namespace Locator.Features.Connections.GetConnection;

internal class GetConnectionQuery(string auth0Id, int clientId, int databaseTypeId)
{
    public string Auth0Id => auth0Id;
    public int ClientId => clientId;
    public int DatabaseTypeId => databaseTypeId;
}

internal sealed class GetConnectionQueryValidator : AbstractValidator<GetConnectionQuery>
{
    public GetConnectionQueryValidator()
    {
        RuleFor(x => x.Auth0Id).NotEmpty().WithMessage("Auth0Id is required");
        RuleFor(x => x.ClientId).NotEmpty().WithMessage("ClientId is required");
        RuleFor(x => x.DatabaseTypeId).NotEmpty().WithMessage("DatabaseTypeId is required");
    }
}

internal class GetConnection(IConnectionRepository connectionRepository)
{
    public async Task<SqlConnection> Handle(GetConnectionQuery query)
    {
        await new GetConnectionQueryValidator().ValidateAndThrowAsync(query);

        return await connectionRepository.GetConnection(
            query.Auth0Id,
            query.ClientId,
            query.DatabaseTypeId
        );
    }
}

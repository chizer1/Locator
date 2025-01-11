using System.Data.SqlClient;
using FluentValidation;
using Locator.Domain;
using Locator.Features.Clients;
using Locator.Features.Users;

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

internal class GetConnection(
    IConnectionRepository connectionRepository,
    IClientRepository clientRepository,
    IUserRepository userRepository
)
{
    public async Task<SqlConnection> Handle(GetConnectionQuery query)
    {
        await new GetConnectionQueryValidator().ValidateAndThrowAsync(query);

        var client = await clientRepository.GetClient(query.ClientId);
        if (client.Status == Status.Inactive)
            throw new Exception("Cannot connect to database since this client is inactive.");

        var user = await userRepository.GetUserByAuth0Id(query.Auth0Id);
        if (user.Status == Status.Inactive)
            throw new Exception("Cannot connect to database since this user is inactive.");

        return await connectionRepository.GetSqlConnection(
            query.Auth0Id,
            query.ClientId,
            query.DatabaseTypeId
        );
    }
}

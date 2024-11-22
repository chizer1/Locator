using FluentValidation;
using Locator.Domain;

namespace Locator.Features.Users.GetUserLogs;

internal class GetUserLogsQuery(string auth0Id)
{
    public string Auth0Id => auth0Id;
}

internal sealed class GetUserLogsQueryValidator : AbstractValidator<GetUserLogsQuery>
{
    public GetUserLogsQueryValidator()
    {
        RuleFor(x => x.Auth0Id).NotEmpty().WithMessage("Auth0Id is required");
    }
}

internal class GetUserLogs(IAuth0UserService auth0UserService)
{
    public async Task<List<UserLog>> Handle(GetUserLogsQuery query)
    {
        await new GetUserLogsQueryValidator().ValidateAndThrowAsync(query);

        return await auth0UserService.GetUserLogs(query.Auth0Id);
    }
}

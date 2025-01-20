using FluentValidation;
using Locator.Domain;

namespace Locator.Features.Roles.GetRoles;

internal class GetRolesQuery() { }

internal sealed class GetRolesQueryValidator : AbstractValidator<GetRolesQuery>
{
    public GetRolesQueryValidator() { }
}

internal class GetRoles(IRoleRepository roleRepository)
{
    public async Task<List<Role>> Handle(GetRolesQuery query)
    {
        await new GetRolesQueryValidator().ValidateAndThrowAsync(query);

        return await roleRepository.GetRoles();
    }
}

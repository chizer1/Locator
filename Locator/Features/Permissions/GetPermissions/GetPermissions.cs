using FluentValidation;
using Locator.Domain;

namespace Locator.Features.Permissions.GetPermissions;

internal class GetPermissionsQuery() { }

internal sealed class GetPermissionsQueryValidator : AbstractValidator<GetPermissionsQuery>
{
    public GetPermissionsQueryValidator() { }
}

internal class GetPermissions(IPermissionRepository permissionRepository)
{
    public async Task<List<Permission>> Handle(GetPermissionsQuery query)
    {
        await new GetPermissionsQueryValidator().ValidateAndThrowAsync(query);

        return await permissionRepository.GetPermissions();
    }
}

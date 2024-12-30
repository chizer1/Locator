using System.Text;
using Locator.Common;
using Locator.Domain;
using Newtonsoft.Json;

namespace Locator.Features.RolePermissions;

internal class Auth0RolePermissionService(Auth0 auth0) : IAuth0RolePermissionService
{
    public async Task UpdateRolePermissions(string auth0RoleId, List<Permission> permissions)
    {
        var accessToken = await auth0.CreateAccessToken();

        var rolePermissionsPayload = permissions
            .Select(permission => new Dictionary<string, string>
            {
                ["permission_name"] = permission.Name,
                ["resource_server_identifier"] = auth0.GetApiIdentifier(),
            })
            .ToList();

        var payload = new Dictionary<string, object> { ["permissions"] = rolePermissionsPayload };
        var jsonContent = JsonConvert.SerializeObject(payload);

        var requestUri = $"https://{auth0.GetAuth0Domain()}/api/v2/roles/{auth0RoleId}/permissions";
        HttpRequestMessage request =
            new(HttpMethod.Post, requestUri)
            {
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json"),
            };

        request.Headers.Add("Authorization", $"Bearer {accessToken}");

        using var response = await new HttpClient().SendAsync(request);
        var responseString = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Auth0 Exception. Failed to add permission: {responseString}");
    }
}

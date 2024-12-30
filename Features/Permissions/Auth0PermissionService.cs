using System.Text;
using Locator.Common;
using Locator.Db;
using Locator.Domain;
using Newtonsoft.Json;

namespace Locator.Features.Permissions;

internal class Auth0PermissionService(Auth0 auth0) : IAuth0PermissionService
{
    public async Task UpdatePermissions(List<Permission> permissions)
    {
        var accessToken = await auth0.CreateAccessToken();

        var scopes = permissions
            .Select(permission => new Dictionary<string, string>
            {
                ["value"] = permission.Name,
                ["description"] = permission.Description,
            })
            .ToList();

        var payload = new Dictionary<string, object> { ["scopes"] = scopes };

        var jsonContent = JsonConvert.SerializeObject(payload);
        var requestUri =
            $"https://{auth0.GetAuth0Domain()}/api/v2/resource-servers/{auth0.GetApiId()}";
        HttpRequestMessage request =
            new(HttpMethod.Patch, requestUri)
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

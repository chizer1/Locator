using System.Text;
using Locator.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Locator.Features.Roles;

internal class Auth0RoleService(Auth0 auth0) : IAuth0RoleService
{
    public async Task<string> AddRole(string roleName, string roleDescription)
    {
        var accessToken = await auth0.CreateAccessToken();

        dynamic addRolePayload = new { roleName, roleDescription };
        var jsonContent = JsonConvert.SerializeObject(addRolePayload);

        var requestUri = $"https://{auth0.GetAuth0Domain()}/api/v2/roles";
        HttpRequestMessage request =
            new(HttpMethod.Post, requestUri)
            {
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json"),
            };
        request.Headers.Add("Authorization", $"Bearer {accessToken}");

        using var response = await new HttpClient().SendAsync(request);
        var responseString = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Auth0 Exception. Failed to add role: {responseString}");

        return JObject.Parse(responseString)["id"]?.ToString();
    }

    public async Task DeleteRole(string auth0RoleId)
    {
        var accessToken = await auth0.CreateAccessToken();

        var requestUri = $"https://{auth0.GetAuth0Domain()}/api/v2/roles/{auth0RoleId}";
        HttpRequestMessage request = new(HttpMethod.Delete, requestUri);

        request.Headers.Add("Authorization", $"Bearer {accessToken}");

        var response = await new HttpClient().SendAsync(request);
        var responseString = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Auth0 Exception. Failed to delete role: {responseString}");
    }

    public async Task UpdateRole(string auth0RoleId, string roleName, string roleDescription)
    {
        var accessToken = await auth0.CreateAccessToken();

        dynamic updateRolePayload = new { roleName, roleDescription };
        var jsonContent = JsonConvert.SerializeObject(updateRolePayload);

        var requestUri = $"https://{auth0.GetAuth0Domain()}/api/v2/roles/{auth0RoleId}";
        HttpRequestMessage request =
            new(HttpMethod.Patch, requestUri)
            {
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json"),
            };
        request.Headers.Add("Authorization", $"Bearer {accessToken}");

        var response = await new HttpClient().SendAsync(request);
        var responseString = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Auth0 Exception. Failed to update role: {responseString}");
    }
}

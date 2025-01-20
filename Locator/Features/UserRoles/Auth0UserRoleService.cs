using System.Text;
using Locator.Common;
using Newtonsoft.Json;

namespace Locator.Features.UserRoles;

internal class Auth0UserRoleService(Auth0 auth0) : IAuth0UserRoleService
{
    public async Task AddUserRole(string auth0Id, string auth0RoleId)
    {
        var accessToken = await auth0.CreateAccessToken();

        dynamic role = new { roles = new[] { auth0RoleId } };
        string jsonContent = JsonConvert.SerializeObject(role);

        var requestUri = $"https://{auth0.GetAuth0Domain()}/api/v2/users/{auth0Id}/roles";
        HttpRequestMessage request =
            new(HttpMethod.Post, requestUri)
            {
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json"),
            };
        request.Headers.Add("Authorization", $"Bearer {accessToken}");

        using var response = await new HttpClient().SendAsync(request);
        var responseString = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Auth0 Exception. Failed to add user to role: {responseString}");
    }

    public async Task DeleteUserRole(string auth0Id, string auth0RoleId)
    {
        var accessToken = await auth0.CreateAccessToken();

        dynamic role = new { roles = new[] { auth0RoleId } };
        string jsonContent = JsonConvert.SerializeObject(role);

        var requestUri = $"https://{auth0.GetAuth0Domain()}/api/v2/users/{auth0Id}/roles";
        HttpRequestMessage request =
            new(HttpMethod.Delete, requestUri)
            {
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json"),
            };
        request.Headers.Add("Authorization", $"Bearer {accessToken}");

        using var response = await new HttpClient().SendAsync(request);
        var responseString = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception(
                $"Auth0 Exception. Failed to delete user from role: {responseString}"
            );
    }
}

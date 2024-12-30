using System.Text;
using Locator.Common;
using Locator.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Locator.Features.Users;

internal class Auth0UserService(Auth0 auth0) : IAuth0UserService
{
    public async Task<string> AddUser(
        string firstName,
        string lastName,
        string emailAddress,
        string password,
        Status userStatus
    )
    {
        var accessToken = await auth0.CreateAccessToken();

        dynamic addUserPayload = new
        {
            email = emailAddress,
            given_name = firstName,
            family_name = lastName,
            name = $"{firstName} {lastName}",
            password,
            verify_email = false,
            connection = "Username-Password-Authentication",
            blocked = userStatus != Status.Active,
        };
        string jsonContent = JsonConvert.SerializeObject(addUserPayload);

        var requestUri = $"https://{auth0.GetAuth0Domain()}/api/v2/users";
        HttpRequestMessage request =
            new(HttpMethod.Post, requestUri)
            {
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json"),
            };
        request.Headers.Add("Authorization", $"Bearer {accessToken}");

        using var response = await new HttpClient().SendAsync(request);
        var responseString = await response.Content.ReadAsStringAsync();

        string auth0Id;
        if (response.IsSuccessStatusCode)
            auth0Id = JObject.Parse(responseString)["user_id"]?.ToString();
        else
            throw new Exception($"Auth0 Exception. Failed to add user: {responseString}");

        return auth0Id;
    }

    public async Task UpdateUser(
        string auth0Id,
        string firstName,
        string lastName,
        string emailAddress,
        Status userStatus
    )
    {
        var accessToken = await auth0.CreateAccessToken();

        dynamic updateUserPayload = new
        {
            email = emailAddress,
            given_name = firstName,
            family_name = lastName,
            name = $"{firstName} {lastName}",
            blocked = userStatus != Status.Active,
        };
        string jsonContent = JsonConvert.SerializeObject(updateUserPayload);

        var requestUri = $"https://{auth0.GetAuth0Domain()}/api/v2/users/{auth0Id}";
        HttpRequestMessage request =
            new(HttpMethod.Patch, requestUri)
            {
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json"),
            };
        request.Headers.Add("Authorization", $"Bearer {accessToken}");

        using var response = await new HttpClient().SendAsync(request);
        var responseString = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Auth0 Exception. Failed to update user: {responseString}");
    }

    public async Task DeleteUser(string auth0Id)
    {
        var accessToken = await auth0.CreateAccessToken();

        var requestUri = $"https://{auth0.GetAuth0Domain()}/api/v2/users/{auth0Id}";
        HttpRequestMessage request = new(HttpMethod.Delete, requestUri);
        request.Headers.Add("Authorization", $"Bearer {accessToken}");

        using var response = await new HttpClient().SendAsync(request);
        var responseString = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Auth0 Exception. Failed to delete user: {responseString}");
    }

    public async Task UpdateUserPassword(string auth0Id, string password)
    {
        var accessToken = await auth0.CreateAccessToken();

        dynamic updatePasswordPayload = new
        {
            connection = "Username-Password-Authentication",
            password,
        };
        string jsonContent = JsonConvert.SerializeObject(updatePasswordPayload);

        var requestUri = $"https://{auth0.GetAuth0Domain()}/api/v2/users/{auth0Id}";
        HttpRequestMessage request =
            new(HttpMethod.Patch, requestUri)
            {
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json"),
            };
        request.Headers.Add("Authorization", $"Bearer {accessToken}");

        using var response = await new HttpClient().SendAsync(request);
        var responseString = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception(
                $"Auth0 Exception. Failed to update user password: {responseString}"
            );
    }

    public async Task<string> GeneratePasswordChangeTicket(string auth0Id, string redirectUrl)
    {
        var accessToken = await auth0.CreateAccessToken();

        dynamic passwordChangePayload = new { user_id = auth0Id, result_url = redirectUrl };
        string jsonContent = JsonConvert.SerializeObject(passwordChangePayload);

        var requestUri = $"https://{auth0.GetAuth0Domain()}/api/v2/tickets/password-change";
        HttpRequestMessage request =
            new(HttpMethod.Post, requestUri)
            {
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json"),
            };
        request.Headers.Add("Authorization", $"Bearer {accessToken}");

        using var response = await new HttpClient().SendAsync(request);
        var responseString = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception(
                $"Auth0 Exception. Failed to generate password change ticket: {responseString}"
            );

        return JObject.Parse(responseString)["ticket"]?.ToString();
    }
}

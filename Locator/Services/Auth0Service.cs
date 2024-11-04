using System.Text;
using Locator.Models.Read;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Locator.Services;

internal class Auth0Service(
    string auth0Domain,
    string auth0ClientId,
    string auth0ClientSecret,
    string apiId,
    string apiIdentifier
)
{
    public async Task<string> GetAccessToken()
    {
        using HttpClient client = new();

        dynamic requestData = new
        {
            client_id = auth0ClientId,
            client_secret = auth0ClientSecret,
            audience = $"https://{auth0Domain}/api/v2/",
            grant_type = "client_credentials",
        };
        string jsonContent = JsonConvert.SerializeObject(requestData);

        var requestUri = $"https://{auth0Domain}/oauth/token";
        HttpRequestMessage request =
            new(HttpMethod.Post, requestUri)
            {
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json"),
            };

        using var response = await client.SendAsync(request);
        var responseString = await response.Content.ReadAsStringAsync();

        string accessToken;
        if (response.IsSuccessStatusCode)
            accessToken = JObject.Parse(responseString)["access_token"]?.ToString();
        else
            throw new Exception($"Auth0 Exception. Failed to get access token: {responseString}");

        return accessToken;
    }

    public async Task<string> CreateUser(
        string accessToken,
        string emailAddress,
        string firstName,
        string lastName
    )
    {
        using HttpClient client = new();

        dynamic userMetadata = new
        {
            email = emailAddress,
            given_name = firstName,
            family_name = lastName,
            name = $"{firstName} {lastName}",
            password = Guid.NewGuid().ToString(),
            verify_email = false,
            connection = "Username-Password-Authentication",
        };
        string jsonContent = JsonConvert.SerializeObject(userMetadata);

        var requestUri = $"https://{auth0Domain}/api/v2/users";
        HttpRequestMessage request =
            new(HttpMethod.Post, requestUri)
            {
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json"),
            };
        request.Headers.Add("Authorization", $"Bearer {accessToken}");

        using var response = await client.SendAsync(request);
        var responseString = await response.Content.ReadAsStringAsync();

        string auth0Id;
        if (response.IsSuccessStatusCode)
            auth0Id = JObject.Parse(responseString)["user_id"]?.ToString();
        else
            throw new Exception($"Auth0 Exception. Failed to create user: {responseString}");

        return auth0Id;
    }

    public async Task UpdateUserPassword(string accessToken, string auth0Id, string password)
    {
        using HttpClient client = new();

        dynamic passwordData = new { connection = "Username-Password-Authentication", password };
        string jsonContent = JsonConvert.SerializeObject(passwordData);

        var requestUri = $"https://{auth0Domain}/api/v2/users/{auth0Id}";
        HttpRequestMessage request =
            new(HttpMethod.Patch, requestUri)
            {
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json"),
            };
        request.Headers.Add("Authorization", $"Bearer {accessToken}");

        using var response = await client.SendAsync(request);
        var responseString = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception(
                $"Auth0 Exception. Failed to update user password: {responseString}"
            );
    }

    public async Task UpdateUser(
        string accessToken,
        string auth0Id,
        string firstName,
        string lastName,
        string email,
        bool blocked = false
    )
    {
        using HttpClient client = new();

        dynamic userData = new
        {
            email,
            given_name = firstName,
            family_name = lastName,
            name = $"{firstName} {lastName}",
            blocked,
        };
        string jsonContent = JsonConvert.SerializeObject(userData);

        var requestUri = $"https://{auth0Domain}/api/v2/users/{auth0Id}";
        HttpRequestMessage request =
            new(HttpMethod.Patch, requestUri)
            {
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json"),
            };
        request.Headers.Add("Authorization", $"Bearer {accessToken}");

        using var response = await client.SendAsync(request);
        var responseString = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Auth0 Exception. Failed to update user: {responseString}");
    }

    public async Task AssignUserToRole(string accessToken, string auth0Id, string auth0RoleId)
    {
        using HttpClient client = new();

        dynamic role = new { roles = new[] { auth0RoleId } };
        string jsonContent = JsonConvert.SerializeObject(role);

        var requestUri = $"https://{auth0Domain}/api/v2/users/{auth0Id}/roles";
        HttpRequestMessage request =
            new(HttpMethod.Post, requestUri)
            {
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json"),
            };
        request.Headers.Add("Authorization", $"Bearer {accessToken}");

        using var response = await client.SendAsync(request);
        var responseString = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception(
                $"Auth0 Exception. Failed to assign user to role: {responseString}"
            );
    }

    public async Task RemoveUserFromRole(string accessToken, string auth0Id, string auth0RoleId)
    {
        using HttpClient client = new();

        dynamic role = new { roles = new[] { auth0RoleId } };
        string jsonContent = JsonConvert.SerializeObject(role);

        var requestUri = $"https://{auth0Domain}/api/v2/users/{auth0Id}/roles";
        HttpRequestMessage request =
            new(HttpMethod.Delete, requestUri)
            {
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json"),
            };
        request.Headers.Add("Authorization", $"Bearer {accessToken}");

        using var response = await client.SendAsync(request);
        var responseString = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception(
                $"Auth0 Exception. Failed to remove user from role: {responseString}"
            );
    }

    public async Task DeleteUser(string accessToken, string auth0Id)
    {
        using HttpClient client = new();

        var requestUri = $"https://{auth0Domain}/api/v2/users/{auth0Id}";
        HttpRequestMessage request = new(HttpMethod.Delete, requestUri);
        request.Headers.Add("Authorization", $"Bearer {accessToken}");

        using var response = await client.SendAsync(request);
        var responseString = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Auth0 Exception. Failed to delete user: {responseString}");
    }

    public async Task<List<UserLog>> GetUserLogs(string accessToken, string auth0Id)
    {
        using HttpClient client = new();

        var requestUri = $"https://{auth0Domain}/api/v2/users/{auth0Id}/logs";
        HttpRequestMessage request = new(HttpMethod.Get, requestUri);
        request.Headers.Add("Authorization", $"Bearer {accessToken}");

        using var response = await client.SendAsync(request);
        var responseString = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Auth0 Exception. Failed to get user logs: {responseString}");

        var userLogs = JsonConvert.DeserializeObject<List<UserLog>>(responseString);
        foreach (var userLog in userLogs)
        {
            userLog.Type = userLog.Type switch
            {
                "s" => "Successful Login",
                "ss" => "Successful Signup",
                "f" => "Failed Login",
                "fp" => "Failed Login (Incorrect Password)",
                "seacft" => "Success Exchange",
                "slo" => "Successful Sign out",
                "scp" => "Success Change Password",
                "scpr" => "Success Change Password Request",
                "fcp" => "Failed Change Password",
                "sce" => "Success Change Email",
                _ => userLog.Type,
            };
        }

        return userLogs;
    }

    public async Task<string> GeneratePasswordChangeTicket(
        string accessToken,
        string auth0Id,
        string clientUrl
    )
    {
        using HttpClient client = new();

        dynamic passwordChangeData = new { user_id = auth0Id, result_url = clientUrl };
        string jsonContent = JsonConvert.SerializeObject(passwordChangeData);

        var requestUri = $"https://{auth0Domain}/api/v2/tickets/password-change";
        HttpRequestMessage request =
            new(HttpMethod.Post, requestUri)
            {
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json"),
            };
        request.Headers.Add("Authorization", $"Bearer {accessToken}");

        using var response = await client.SendAsync(request);
        var responseString = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception(
                $"Auth0 Exception. Failed to generate password change ticket: {responseString}"
            );

        return JObject.Parse(responseString)["ticket"]?.ToString();
    }

    public async Task<string> AddRole(string accessToken, string name, string description)
    {
        using HttpClient client = new();

        dynamic roleData = new { name, description };
        string jsonContent = JsonConvert.SerializeObject(roleData);

        var requestUri = $"https://{auth0Domain}/api/v2/roles";
        HttpRequestMessage request =
            new(HttpMethod.Post, requestUri)
            {
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json"),
            };
        request.Headers.Add("Authorization", $"Bearer {accessToken}");

        using var response = await client.SendAsync(request);
        var responseString = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Auth0 Exception. Failed to add role: {responseString}");

        return JObject.Parse(responseString)["id"]?.ToString();
    }

    public async Task DeleteRole(string accessToken, string auth0RoleId)
    {
        using HttpClient client = new();

        var requestUri = $"https://{auth0Domain}/api/v2/roles/{auth0RoleId}";
        HttpRequestMessage request = new(HttpMethod.Delete, requestUri);

        request.Headers.Add("Authorization", $"Bearer {accessToken}");

        using var response = await client.SendAsync(request);
        var responseString = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Auth0 Exception. Failed to delete role: {responseString}");
    }

    public async Task UpdateRole(
        string accessToken,
        string auth0RoleId,
        string name,
        string description
    )
    {
        using HttpClient client = new();

        dynamic roleData = new { name, description };
        string jsonContent = JsonConvert.SerializeObject(roleData);

        var requestUri = $"https://{auth0Domain}/api/v2/roles/{auth0RoleId}";
        HttpRequestMessage request =
            new(HttpMethod.Patch, requestUri)
            {
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json"),
            };
        request.Headers.Add("Authorization", $"Bearer {accessToken}");

        using var response = await client.SendAsync(request);
        var responseString = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Auth0 Exception. Failed to update role: {responseString}");
    }

    public async Task UpdatePermissions(string accessToken, List<Permission> permissions)
    {
        using HttpClient client = new();

        var scopes = new List<Dictionary<string, string>>();
        foreach (var permission in permissions)
        {
            scopes.Add(
                new Dictionary<string, string>
                {
                    ["value"] = permission.PermissionName,
                    ["description"] = permission.PermissionDescription,
                }
            );
        }

        var payload = new Dictionary<string, object> { ["scopes"] = scopes };

        string jsonContent = JsonConvert.SerializeObject(payload);
        var requestUri = $"https://{auth0Domain}/api/v2/resource-servers/{apiId}";
        HttpRequestMessage request =
            new(HttpMethod.Patch, requestUri)
            {
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json"),
            };

        request.Headers.Add("Authorization", $"Bearer {accessToken}");

        using var response = await client.SendAsync(request);
        var responseString = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Auth0 Exception. Failed to add permission: {responseString}");
    }

    public async Task AddPermissionToRole(
        string accessToken,
        string permissionName,
        string auth0RoleId
    )
    {
        using HttpClient client = new();

        var payload = new Dictionary<string, object>
        {
            ["permissions"] = new List<Dictionary<string, string>>
            {
                new()
                {
                    ["permission_name"] = permissionName,
                    ["resource_server_identifier"] = apiIdentifier,
                },
            },
        };
        string jsonContent = JsonConvert.SerializeObject(payload);

        var requestUri = $"https://{auth0Domain}/api/v2/roles/{auth0RoleId}/permissions";
        HttpRequestMessage request =
            new(HttpMethod.Post, requestUri)
            {
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json"),
            };

        request.Headers.Add("Authorization", $"Bearer {accessToken}");

        using var response = await client.SendAsync(request);
        var responseString = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Auth0 Exception. Failed to add permission: {responseString}");
    }
}

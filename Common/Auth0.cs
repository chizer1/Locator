using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Locator.Common;

internal class Auth0(
    string auth0Domain,
    string auth0ClientId,
    string auth0ClientSecret,
    string apiId,
    string apiIdentifier
)
{
    public bool IsAuth0Configured()
    {
        return !string.IsNullOrEmpty(auth0Domain)
            && !string.IsNullOrEmpty(auth0ClientId)
            && !string.IsNullOrEmpty(auth0ClientSecret)
            && !string.IsNullOrEmpty(apiId)
            && !string.IsNullOrEmpty(apiIdentifier);
    }

    public string GetAuth0Domain()
    {
        return auth0Domain;
    }

    public string GetApiId()
    {
        return apiId;
    }

    public string GetApiIdentifier()
    {
        return apiIdentifier;
    }

    public async Task<string> CreateAccessToken()
    {
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

        using var response = await new HttpClient().SendAsync(request);
        var responseString = await response.Content.ReadAsStringAsync();

        string accessToken;
        if (response.IsSuccessStatusCode)
            accessToken = JObject.Parse(responseString)["access_token"]?.ToString();
        else
            throw new Exception($"Auth0 Exception. Failed to get access token: {responseString}");

        return accessToken;
    }
}

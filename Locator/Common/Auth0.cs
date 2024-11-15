using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Locator.Common;

public class Auth0(string auth0Domain, string auth0ClientId, string auth0ClientSecret)
{
    public async Task<string> GetAccessToken()
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

using Microsoft.IdentityModel.JsonWebTokens;

namespace Locator.Utilities;

internal class HttpContextUtilities
{
    public string GetAuth0Id(HttpContext httpContext)
    {
        var idToken = GetAccessToken(httpContext);

        if (IsTokenExpired(idToken))
            throw new Exception("Token is expired");

        return GetAuth0Id(idToken);
    }

    private JsonWebToken GetAccessToken(HttpContext httpContext)
    {
        var accessToken = httpContext
            .Request.Headers.Authorization.ToString()
            .Replace("Bearer ", "");

        if (string.IsNullOrEmpty(accessToken))
            throw new Exception("No access token found");

        var handler = new JsonWebTokenHandler();
        if (!handler.CanReadToken(accessToken))
            throw new Exception("Can't read access token");

        return handler.ReadJsonWebToken(accessToken);
    }

    private bool IsTokenExpired(JsonWebToken jsonWebToken)
    {
        return jsonWebToken.ValidTo < DateTime.UtcNow;
    }

    private string GetAuth0Id(JsonWebToken jsonWebToken)
    {
        return jsonWebToken.Claims.First(claim => claim.Type == "sub").Value;
    }
}

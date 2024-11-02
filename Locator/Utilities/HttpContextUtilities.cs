using Microsoft.IdentityModel.JsonWebTokens;

namespace Locator.Utilities;

internal static class HttpContextUtilities
{
    public static string GetAuth0Id(HttpContext httpContext)
    {
        var idToken = GetAccessToken(httpContext);

        if (IsTokenExpired(idToken))
            throw new Exception("Token is expired");

        return GetAuth0Id(idToken);
    }

    private static JsonWebToken GetAccessToken(HttpContext httpContext)
    {
        string accessToken = null;
        if (httpContext.Request.Headers.TryGetValue("Authorization", out var value))
            accessToken = value.ToString().Replace("Bearer ", "");

        var handler = new JsonWebTokenHandler();
        var jsonToken = handler.ReadJsonWebToken(accessToken);

        return jsonToken;
    }

    private static bool IsTokenExpired(JsonWebToken jsonWebToken)
    {
        return jsonWebToken.ValidTo < DateTime.UtcNow;
    }

    private static string GetAuth0Id(JsonWebToken jsonWebToken)
    {
        return jsonWebToken.Claims.First(claim => claim.Type == "sub").Value;
    }
}

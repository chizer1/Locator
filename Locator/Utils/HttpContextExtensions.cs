using Microsoft.IdentityModel.JsonWebTokens;

namespace Locator.Utils;

internal static class HttpContextUtils
{
    public static string GetAuth0Id(HttpContext httpContext)
    {
        var idToken = GetIdToken(httpContext);

        if (IsTokenExpired(idToken))
            throw new Exception("Token is expired");

        return GetAuth0Id(idToken);
    }

    private static JsonWebToken GetIdToken(HttpContext httpContext)
    {
        string idToken = null;
        if (httpContext.Request.Headers.TryGetValue("Authorization", out var value))
            idToken = value.ToString().Replace("Bearer ", "");

        var handler = new JsonWebTokenHandler();
        var jsonToken = handler.ReadJsonWebToken(idToken);

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
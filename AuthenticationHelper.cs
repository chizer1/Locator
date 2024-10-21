using System.IdentityModel.Tokens.Jwt;

namespace Locator;

public class AuthenticationHelper
{
    private static JwtSecurityToken GetToken(HttpContext httpContext)
    {
        string accessToken = null;
        if (httpContext.Request.Headers.TryGetValue("Authorization", out var value))
            accessToken = value.ToString().Replace("Bearer ", "");

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.ReadToken(accessToken) as JwtSecurityToken;
    }

    private static JwtSecurityToken GetToken(string accessToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.ReadToken(accessToken) as JwtSecurityToken;
    }

    public string Auth0Id(HttpContext httpContext)
    {
        return GetToken(httpContext).Claims.First(claim => claim.Type == "sub").Value;
    }

    private IEnumerable<string> Roles(HttpContext httpContext)
    {
        return GetToken(httpContext).Claims.First(claim => claim.Type == "scope").Value.Split(" ");
    }

    public bool IsUserInRole(HttpContext httpContext, string role)
    {
        return Roles(httpContext).Contains(role);
    }

    public bool IsValidAccessToken(string accessToken)
    {
        return GetToken(accessToken).ValidTo > DateTime.UtcNow;
    }
}

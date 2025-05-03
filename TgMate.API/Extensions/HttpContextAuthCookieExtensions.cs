using TgMate.API.Auth;

namespace TgMate.API.Extensions;

public static class HttpContextAuthCookieExtensions
{
  public static void SetAuthCookie(this HttpContext httpContext, EncodedAccessToken token)
  {
    httpContext.Response.Cookies.Append(
      AuthContext.GetAccessTokenName(),
      token.Token,
      new CookieOptions
      {
        Path = "/",
        SameSite = SameSiteMode.None,
        Expires = token.ExpiredAt,
        HttpOnly = true,
        Secure = true,
        Domain = httpContext.Request.Host.Host,
      }
    );
  }
}

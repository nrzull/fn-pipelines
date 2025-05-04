using TgMate.API.Web.Auth.Login;

namespace TgMate.API.Web.Auth;

public static class AuthExtensions
{
  public static void UseAuthEndpoints(this WebApplicationBuilder builder)
  {
    builder.Services.AddScoped<LoginEndpoint>();
  }
}

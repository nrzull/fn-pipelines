using TgMate.API.Auth;

namespace TgMate.API.Web.Auth.Login;

public abstract record LoginResponse
{
  public record Ok(EncodedAccessToken Data) : LoginResponse;
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TgMate.API.Web.Auth.Login;

namespace TgMate.API.Web.Auth;

public class AuthHub(ILogger<AuthHub> logger) : Hub
{
  public async Task Login(string phone, [FromServices] LoginEndpoint endpoint)
  {
    try
    {
      var _ = await endpoint.Handle(new(phone), Context.ConnectionId) switch
      {
        LoginResponse.Ok ok => new Func<Task>(async () =>
        {
          await Clients.Caller.SendAsync("LoginOK", ok);
        })(),
        _ => new Func<Task>(async () =>
        {
          await Clients.Caller.SendAsync("LoginERR");
        })(),
      };
    }
    catch (Exception e)
    {
      logger.LogError(e, "Login");
    }
  }
}

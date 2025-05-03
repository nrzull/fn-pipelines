using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TgMate.API.Extensions;

namespace TgMate.API.Auth;

public class AuthHub : Hub
{
  public async Task Login([FromServices] AuthContext authContext, string phone)
  {
    var token = await authContext.Login(new(phone), Context.ConnectionId);
    var httpContext = Context.GetHttpContext();
    httpContext?.SetAuthCookie(token);
    await Clients.Caller.SendAsync("LoginOK");
  }
}

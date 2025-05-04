using Microsoft.AspNetCore.SignalR;
using TgMate.API.Auth;
using TgMate.API.Core;
using TgMate.API.Telegram;

namespace TgMate.API.Web.Auth.Login;

public class LoginEndpoint(AuthContext authContext, TgContext tgContext, IHubContext<AuthHub> hub)
{
  public async Task<LoginResponse> Handle(Phone phone, string connectionId)
  {
    var hubClient = hub.Clients.Client(connectionId);
    var cts = new CancellationTokenSource();

    var tgWorkerClient = await tgContext.EnsureTgWorkerClient(
      phone.Value,
      () => hubClient.InvokeAsync<string?>("Login", cts.Token)
    );

    var tgWorker = await tgContext.EnsureTgWorker(phone);

    return new LoginResponse.Ok(authContext.EncodeAccessToken(tgWorker));
  }
}

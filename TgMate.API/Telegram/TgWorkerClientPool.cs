using System.Collections.Concurrent;
using WTelegram;

namespace TgMate.API.Telegram;

public class TgWorkerClientPool()
{
  static ConcurrentDictionary<string, Client> Pool { get; set; } = [];

  public Client? Get(string phone)
  {
    Pool.TryGetValue(phone, out var existingClient);
    return existingClient;
  }

  public async Task Destroy(string phone)
  {
    Pool.Remove(phone, out var existingClient);
    if (existingClient is { })
    {
      await existingClient.ResetAsync(true, true);
      await existingClient.DisposeAsync();
    }
  }

  public async Task<Client> Ensure(
    string phone,
    Func<Task<string?>> getVerificationCode,
    bool forceLogin = true
  )
  {
    if (Pool.TryGetValue(phone, out var existingClient))
    {
      await Login(phone, existingClient, getVerificationCode, forceLogin);
      return existingClient;
    }

    var client = new Client(
      int.Parse(Environment.GetEnvironmentVariable("TELEGRAM_APP_API_ID")!),
      Environment.GetEnvironmentVariable("TELEGRAM_APP_API_HASH"),
      sessionPathname: $"./bin/phone{phone}.session"
    )
    { };

    client = Pool.GetOrAdd(phone, client);
    await Login(phone, client, getVerificationCode, forceLogin);

    return client;
  }

  async Task Login(
    string phone,
    Client client,
    Func<Task<string?>> getVerificationCode,
    bool forceLogin
  )
  {
    string? requestedInfo = phone;

    if (forceLogin)
    {
      await client.ResetAsync(true, true);
    }

    while (client.User == null)
    {
      var result = await client.Login(requestedInfo);

      switch (result)
      {
        case "verification_code":
          requestedInfo = await getVerificationCode();
          break;

        default:
          requestedInfo = null;
          break;
      }
    }
  }
}

using JWT.Algorithms;
using JWT.Builder;
using TgMate.API.Telegram;

namespace TgMate.API.Auth;

public class AuthContext()
{
  public EncodedAccessToken EncodeAccessToken(TgWorker tgWorker)
  {
    var expiredAt = DateTimeOffset.UtcNow.AddHours(12);

    var token = JwtBuilder
      .Create()
      .WithAlgorithm(new HMACSHA256Algorithm())
      .WithSecret(GetTokenSecret())
      .AddClaim("exp", expiredAt.ToUnixTimeSeconds())
      .AddClaim("tg_worker_id", tgWorker.Id)
      .Encode();

    return new EncodedAccessToken(Token: token, ExpiredAt: expiredAt);
  }

  public DecodedAccessToken DecodeAccessToken(string token)
  {
    var decoded = JwtBuilder
      .Create()
      .WithAlgorithm(new HMACSHA256Algorithm())
      .WithSecret(GetTokenSecret())
      .MustVerifySignature()
      .Decode<IDictionary<string, System.Text.Json.JsonElement>>(token);

    decoded.TryGetValue("exp", out var _expiredAtSeconds);
    decoded.TryGetValue("tg_worker_id", out var _tgWorkerId);

    if (!_expiredAtSeconds.TryGetInt64(out long expiredAtSeconds))
    {
      throw new Exception("exp is invalid");
    }

    if (!_tgWorkerId.TryGetGuid(out var tgWorkerId))
    {
      throw new Exception("tg_worker_id is invalid");
    }

    return new DecodedAccessToken(
      TgWorkerId: tgWorkerId,
      ExpiredAt: DateTimeOffset.FromUnixTimeSeconds(expiredAtSeconds)
    );
  }

  public static string GetAccessTokenName() => "tgmate_access_token";

  string? GetTokenSecret() => Environment.GetEnvironmentVariable("API_AUTH_SECRET");
}

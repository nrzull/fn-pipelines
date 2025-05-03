namespace TgMate.API.Auth;

public record EncodedAccessToken(string Token, DateTimeOffset ExpiredAt);

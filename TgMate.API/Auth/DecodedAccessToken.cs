namespace TgMate.API.Auth;

public record DecodedAccessToken(Guid TgWorkerId, DateTimeOffset ExpiredAt);

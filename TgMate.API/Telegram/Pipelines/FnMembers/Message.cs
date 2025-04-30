namespace TgMate.API.Telegram.Pipelines.FnMembers;

public class Message : FnMember
{
  public required TelegramMessageDTO Value { get; set; }
};

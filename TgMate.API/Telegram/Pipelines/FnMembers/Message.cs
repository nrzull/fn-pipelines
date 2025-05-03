namespace TgMate.API.Telegram.Pipelines.FnMembers;

public class Message : FnMember
{
  public required TgMessageDTO Value { get; set; }
};

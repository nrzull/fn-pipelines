using TgMate.API.Telegram.Pipelines.FnMembers;

namespace TgMate.API.Telegram.Pipelines.Fns.Messages;

public class GetMessageFn : Fn
{
  public override List<FnMember> Args { get; set; } = [];

  public override Task<List<FnMember>> Execute()
  {
    {
      if (Args is [IdAccessHash { } idAccessHash]) { }
    }

    return Task.Run(() => new List<FnMember> { });
  }
};

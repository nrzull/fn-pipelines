using TgMate.API.Telegram.Pipelines.FnMembers;

namespace TgMate.API.Telegram.Pipelines.Fns.Messages;

public class FilterByTextFn : Fn
{
  public override List<FnMember> Args { get; set; } = [];

  public override Task<List<FnMember>> Execute()
  {
    {
      if (Args is [IdAccessHash { } idAccessHash, Str { } str]) { }
    }

    {
      if (Args is [Message { } msg, Str { } str]) { }
    }

    return Task.Run(() => new List<FnMember> { });
  }
};

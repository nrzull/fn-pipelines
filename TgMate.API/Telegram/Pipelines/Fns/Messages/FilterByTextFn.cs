using TgMate.API.Telegram.Pipelines.FnMembers;

namespace TgMate.API.Telegram.Pipelines.Fns.Messages;

public class FilterByTextFn : Fn
{
  public override async Task<List<FnMember>> Run() =>
    Input switch
    {
      [IdAccessHash { } idAccessHash, Str { } str] => await Handle(idAccessHash, str),
      [Message { } msg, Str { } str] => await Handle(msg, str),
      _ => throw new Exception("invalid args"),
    };

  public Task<List<FnMember>> Handle(Message msg, Str str)
  {
    return Task.Run<List<FnMember>>(() => []);
  }

  public Task<List<FnMember>> Handle(IdAccessHash idAccessHash, Str str)
  {
    return Task.Run<List<FnMember>>(() => []);
  }
};

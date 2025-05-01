using TgMate.API.Telegram.Pipelines.FnMembers;

namespace TgMate.API.Telegram.Pipelines.Fns.Channels;

public class GetChannelFn : Fn
{
  public override async Task<List<FnMember>> Run() =>
    Input switch
    {
      [IdAccessHash { } idAccessHash] => await Handle(idAccessHash),
      _ => throw new Exception("invalid args"),
    };

  Task<List<FnMember>> Handle(IdAccessHash idAccessHash)
  {
    return Task.Run<List<FnMember>>(() => []);
  }
};

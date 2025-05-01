using TgMate.API.Telegram.Pipelines.FnMembers;

namespace TgMate.API.Telegram.Pipelines.Fns.Messages;

public class HasTextFn : Fn
{
  public override async Task<List<FnMember>> Run() =>
    Input switch
    {
      [Message { } msg, Str { } str] => await Handle(msg, str),
      _ => throw new Exception("invalid args"),
    };

  Task<List<FnMember>> Handle(Message msg, Str str) =>
    Task.Run<List<FnMember>>(
      () => [new Bool() { Value = msg.Value.Text is { } text && text.Contains(str.Value) }]
    );
};

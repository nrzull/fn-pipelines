using TgMate.API.Telegram.Pipelines.FnMembers;

namespace TgMate.API.Telegram.Pipelines.Fns.Strings;

public class HasSubstringFn : Fn
{
  public override async Task<List<FnMember>> Run() =>
    Input switch
    {
      [Str { } msg, Str { } str] => await Handle(msg, str),
      _ => throw new FnMembersException(Input),
    };

  Task<List<FnMember>> Handle(Str v1, Str v2) =>
    Task.Run<List<FnMember>>(
      () => [new Bool() { Value = v1.Value is { } text && text.Contains(v2.Value) }]
    );
};

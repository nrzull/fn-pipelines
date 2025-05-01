using TgMate.API.Telegram.Pipelines.FnMembers;

namespace TgMate.API.Telegram.Pipelines.Fns.Flow;

public class CondFn : Fn
{
  public override async Task<List<FnMember>> Run() =>
    Input switch
    {
      [Fn { } fn, Fn { } trueFn, Fn { } falseFn] => await Handle(fn, trueFn, falseFn),
      _ => throw new Exception("expect fn, true clause and false clause"),
    };

  async Task<List<FnMember>> Handle(Fn fn, Fn trueFn, Fn falseFn)
  {
    if ((await fn.Run()) is [Bool { Value: bool value }])
    {
      return value ? await trueFn.Run() : await falseFn.Run();
    }
    else
    {
      throw new Exception("expect Bool");
    }
  }
}

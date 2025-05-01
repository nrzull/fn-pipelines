namespace TgMate.API.Telegram.Pipelines.Fns.Flow;

public class InputFn : Fn
{
  public override Task<List<FnMember>> Run() => Task.Run(() => Input);
}

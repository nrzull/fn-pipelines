using System.Text.Json.Serialization;
using TgMate.API.Telegram.Pipelines.Fns.Flow;
using TgMate.API.Telegram.Pipelines.Fns.Messages;

namespace TgMate.API.Telegram.Pipelines;

[JsonPolymorphic]
[JsonDerivedType(typeof(GetMessageFn), nameof(GetMessageFn))]
[JsonDerivedType(typeof(FilterByTextFn), nameof(FilterByTextFn))]
[JsonDerivedType(typeof(HasTextFn), nameof(HasTextFn))]
[JsonDerivedType(typeof(CondFn), nameof(CondFn))]
[JsonDerivedType(typeof(InputFn), nameof(InputFn))]
public class Fn : FnMember
{
  public List<FnMember> Input { get; set; } = [];

  public virtual Task<List<FnMember>> Run() => Task.Run<List<FnMember>>(() => []);
};

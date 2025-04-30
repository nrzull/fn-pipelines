using System.Text.Json.Serialization;
using TgMate.API.Telegram.Pipelines.Fns.Messages;

namespace TgMate.API.Telegram.Pipelines;

[JsonPolymorphic]
[JsonDerivedType(typeof(GetMessageFn), nameof(GetMessageFn))]
[JsonDerivedType(typeof(FilterByTextFn), nameof(FilterByTextFn))]
public class Fn : FnMember
{
  public virtual List<FnMember> Args { get; set; } = [];

  public virtual Task<List<FnMember>> Execute() => Task.Run(() => new List<FnMember> { });
};

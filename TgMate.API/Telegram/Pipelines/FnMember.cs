using System.Text.Json.Serialization;
using TgMate.API.Telegram.Pipelines.FnMembers;

namespace TgMate.API.Telegram.Pipelines;

[JsonPolymorphic]
[JsonDerivedType(typeof(IdAccessHash), nameof(IdAccessHash))]
[JsonDerivedType(typeof(Message), nameof(Message))]
[JsonDerivedType(typeof(Str), nameof(Str))]
[JsonDerivedType(typeof(Int), nameof(Int))]
[JsonDerivedType(typeof(Bool), nameof(Bool))]
[JsonDerivedType(typeof(Fn), nameof(Fn))]
public class FnMember;

[JsonSerializable(typeof(List<FnMember>))]
public partial class FnMemberJsonSerializerContext : JsonSerializerContext;

using System.Text.Json.Serialization;
using TgMate.API.Telegram.Pipelines.FnMembers;
using TgMate.API.Telegram.Pipelines.Fns.Flow;
using TgMate.API.Telegram.Pipelines.Fns.Messages;
using TgMate.API.Telegram.Pipelines.Fns.Strings;

namespace TgMate.API.Telegram.Pipelines;

[JsonPolymorphic]
[JsonDerivedType(typeof(IdAccessHash), nameof(IdAccessHash))]
[JsonDerivedType(typeof(Message), nameof(Message))]
[JsonDerivedType(typeof(Str), nameof(Str))]
[JsonDerivedType(typeof(Int), nameof(Int))]
[JsonDerivedType(typeof(Bool), nameof(Bool))]
[JsonDerivedType(typeof(Fn), nameof(Fn))]
[JsonDerivedType(typeof(GetMessageFn), nameof(GetMessageFn))]
[JsonDerivedType(typeof(FilterByTextFn), nameof(FilterByTextFn))]
[JsonDerivedType(typeof(HasTextFn), nameof(HasTextFn))]
[JsonDerivedType(typeof(CondFn), nameof(CondFn))]
[JsonDerivedType(typeof(InputFn), nameof(InputFn))]
[JsonDerivedType(typeof(HasSubstringFn), nameof(HasSubstringFn))]
[JsonDerivedType(typeof(UnitReference), nameof(UnitReference))]
public class FnMember;

[JsonSourceGenerationOptions(
  WriteIndented = false,
  PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
  PropertyNameCaseInsensitive = true,
  RespectNullableAnnotations = true,
  RespectRequiredConstructorParameters = true,
  AllowOutOfOrderMetadataProperties = true,
  UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip
)]
[JsonSerializable(typeof(List<FnMember>))]
public partial class FnMemberJsonSerializerContext : JsonSerializerContext;

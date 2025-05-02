using System.Text.Json;

namespace TgMate.API.Telegram.Pipelines;

public class FnMembersException(List<FnMember> fnMembers)
  : Exception(
    $"invalid fn member args: {JsonSerializer.Serialize(fnMembers, FnMemberJsonSerializerContext.Default.ListFnMember)}"
  );

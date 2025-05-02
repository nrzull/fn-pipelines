namespace TgMate.API.Telegram.Pipelines;

public record NewUnitDTO(
  string Key,
  Fn Fn,
  List<FnMember> Input,
  string? InputHolderKey,
  string? ParentKey
);

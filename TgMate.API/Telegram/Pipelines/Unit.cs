namespace TgMate.API.Telegram.Pipelines;

public class Unit
{
  public Guid Id { get; set; }
  public required string Key { get; set; }

  public required DateTimeOffset CreatedAt { get; set; }
  public DateTimeOffset? StartedAt { get; set; }
  public DateTimeOffset? FinishedAt { get; set; }

  public required UnitStatus Status { get; set; }

  public required Fn Fn { get; set; }
  public List<FnMember> Input { get; set; } = [];
  public List<FnMember> Output { get; set; } = [];

  public Guid? ParentId { get; set; }
  public Unit? Parent { get; set; }

  public Guid? InputHolderId { get; set; }
  public Unit? InputHolder { get; set; }

  public List<Unit> Children { get; set; } = [];

  public Guid PipelineId { get; set; }
  public required Pipeline Pipeline { get; set; }
}

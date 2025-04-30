namespace TgMate.API.Telegram.Pipelines;

public class Pipeline
{
  public Guid Id { get; set; }

  public required DateTimeOffset CreatedAt { get; set; }
  public DateTimeOffset? ScheduledAt { get; set; }
  public DateTimeOffset? CancelledAt { get; set; }

  public List<Unit> Units { get; set; } = [];
}

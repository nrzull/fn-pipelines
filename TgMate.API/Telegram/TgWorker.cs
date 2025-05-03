using TgMate.API.Telegram.Pipelines;

namespace TgMate.API.Telegram;

public class TgWorker
{
  public Guid Id { get; set; }

  public required string Phone { get; set; }
  public required DateTimeOffset CreatedAt { get; set; }

  public List<Pipeline> Pipelines { get; set; } = [];
}

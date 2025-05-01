using Microsoft.EntityFrameworkCore;

namespace TgMate.API.Telegram.Pipelines;

public class PipelinesContext(ApplicationDbContext dbContext)
{
  public async Task<Pipeline> CreatePipeline()
  {
    var pipeline = new Pipeline() { CreatedAt = DateTimeOffset.UtcNow };

    dbContext.Add(pipeline);
    await dbContext.SaveChangesAsync();

    return pipeline;
  }

  public async Task<List<Pipeline>> GetPipelines() =>
    await dbContext.TelegramPipelines.ToListAsync();
}

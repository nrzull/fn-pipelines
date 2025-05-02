using Microsoft.EntityFrameworkCore;

namespace TgMate.API.Telegram.Pipelines;

public class PipelinesContext(ApplicationDbContext dbContext)
{
  public async Task<Pipeline> CreatePipeline(List<NewUnitDTO> newUnits)
  {
    var pipeline = new Pipeline() { CreatedAt = DateTimeOffset.UtcNow };

    var units = newUnits
      .Select(v => new Unit()
      {
        Key = v.Key,
        Fn = v.Fn,
        Input = v.Input,
        Status = UnitStatus.Created,
        CreatedAt = DateTimeOffset.UtcNow,
        Pipeline = pipeline,
      })
      .ToList();

    foreach (var newUnit in newUnits)
    {
      var unit = units.Single(v => v.Key == newUnit.Key);

      if (newUnit.ParentKey is string { Length: > 0 } p)
      {
        unit.Parent = units.Single(v => v.Key == p);
      }

      if (newUnit.InputHolderKey is string { Length: > 0 } s)
      {
        unit.InputHolder = units.Single(v => v.Key == s);
      }
    }

    dbContext.Add(pipeline);
    dbContext.AddRange(units);
    await dbContext.SaveChangesAsync();

    return pipeline;
  }

  public async Task<List<Pipeline>> GetPipelines() =>
    await dbContext.TelegramPipelines.Include(v => v.Units).ToListAsync();
}

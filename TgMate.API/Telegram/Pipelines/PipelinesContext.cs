using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using TgMate.API.Telegram.Pipelines.FnMembers;

namespace TgMate.API.Telegram.Pipelines;

public class PipelinesContext(ApplicationDbContext dbContext, ILogger<PipelinesContext> logger)
{
  public async Task<Pipeline> CreatePipeline(List<NewUnitDTO> newUnits)
  {
    using var activity = Tracing.Source.StartActivity("CreatePipeline");

    if (newUnits.Count() == 0)
    {
      throw new Exception("empty units");
    }

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

    if (units.Where(v => v.Parent is null).Count() > 1)
    {
      throw new Exception("only one unit can be without a parent");
    }

    dbContext.Add(pipeline);
    dbContext.AddRange(units);
    await dbContext.SaveChangesAsync();

    activity?.SetTag("pipelineId", pipeline.Id);

    return pipeline;
  }

  public async Task<List<Pipeline>> GetPipelines() =>
    await dbContext.TelegramPipelines.ToListAsync();

  public async Task<Pipeline?> GetPipeline(Guid id) =>
    await dbContext
      .TelegramPipelines.Where(v => v.Id == id)
      .Include(v => v.Units)
      .FirstOrDefaultAsync();

  public async Task StartPipeline(Guid id)
  {
    var pipeline = await GetPipeline(id);
    if (pipeline is null)
    {
      throw new Exception("invalid pipeline");
    }

    var unit = pipeline.Units.Single(v => v.ParentId is null);

    await ProcessUnit(unit.Id, unit.PipelineId);
  }

  public async Task<Unit?> GetUnitById(Guid id, Guid pipelineId) =>
    await dbContext
      .TelegramPipelineUnits.Where(v => v.Id == id && v.PipelineId == pipelineId)
      .FirstOrDefaultAsync();

  public async Task<Unit?> GetUnitByKey(string key, Guid pipelineId) =>
    await dbContext
      .TelegramPipelineUnits.Where(v => v.Key == key && v.PipelineId == pipelineId)
      .FirstOrDefaultAsync();

  public async Task<List<Unit>> GetUnitChildren(Unit parent) =>
    await dbContext
      .TelegramPipelineUnits.Where(v =>
        v.ParentId == parent.Id && v.PipelineId == parent.PipelineId
      )
      .ToListAsync();

  public async Task ProcessUnit(Guid unitId, Guid pipelineId)
  {
    using var activity = Tracing.Source.StartActivity($"ProcessUnit");
    activity?.SetTag("unitId", unitId);
    activity?.SetTag("pipelineId", pipelineId);

    var unit = await GetUnitById(unitId, pipelineId);
    if (unit is null)
    {
      throw new Exception("invalid unit");
    }

    if (
      unit.InputHolderId is { } inputHolderId
      && await GetUnitById(inputHolderId, unit.PipelineId) is var inputHolder
      && inputHolder is { Output: { } inputFromHolder }
    )
    {
      unit.Fn.Input = [.. inputFromHolder, .. unit.Input];
    }
    else
    {
      unit.Fn.Input = unit.Input;
    }

    dbContext.Update(unit);
    unit.Status = UnitStatus.InProgress;
    unit.StartedAt = DateTimeOffset.UtcNow;
    await dbContext.SaveChangesAsync();

    var result = await unit.Fn.Run();
    unit.Output = result;
    unit.Status = UnitStatus.Done;
    unit.FinishedAt = DateTimeOffset.UtcNow;

    await dbContext.SaveChangesAsync();

    var nextUnit = await FindNextUnit(unit);

    logger.Fn(JsonSerializer.Serialize(unit.Fn, FnMemberJsonSerializerContext.Default.Fn));

    logger.FnOutput(
      JsonSerializer.Serialize(result, FnMemberJsonSerializerContext.Default.ListFnMember)
    );

    if (nextUnit is { })
    {
      await ProcessUnit(nextUnit.Id, nextUnit.PipelineId);
    }
  }

  public async Task<Unit?> FindNextUnit(Unit current)
  {
    if (current.Output is [UnitReference { Key: string { Length: > 0 } key }])
    {
      var nextUnit = await GetUnitByKey(key, current.PipelineId);
      if (nextUnit is null || nextUnit.ParentId != current.Id)
      {
        throw new Exception("invalid unit reference");
      }

      return nextUnit;
    }

    var children = await GetUnitChildren(current);
    if (children.Count() is var c && c > 1)
    {
      throw new Exception($"should be the only one child of parent unit. Got: {c}");
    }

    return children.FirstOrDefault();
  }
}

internal static partial class LoggerExtensions
{
  [LoggerMessage(LogLevel.Information, "fnOutput {@fnOutput}")]
  public static partial void FnOutput(this ILogger logger, string fnOutput);

  [LoggerMessage(LogLevel.Information, "fn {@fn}")]
  public static partial void Fn(this ILogger logger, string fn);
}

using Microsoft.Extensions.DependencyInjection;
using TgMate.API.Telegram.Pipelines;
using TgMate.API.Telegram.Pipelines.FnMembers;
using TgMate.API.Telegram.Pipelines.Fns.Flow;
using TgMate.API.Telegram.Pipelines.Fns.Messages;
using TgMate.API.Telegram.Pipelines.Fns.Strings;

namespace TgMate.API.Test.Telegram.Pipelines;

public class PipelinesContextTest : CustomLifetime
{
  [Fact]
  public async Task CreatePipeline() =>
    await WithScope(
      async (scope, dbContext) =>
      {
        var pipelinesContext = scope.ServiceProvider.GetRequiredService<PipelinesContext>();

        List<NewUnitDTO> newUnits =
        [
          new(
            Key: "a",
            Fn: new InputFn() { },
            Input: [new Str() { Value = "hello world" }],
            ParentKey: null,
            InputHolderKey: null
          ),
          new(
            Key: "b",
            Fn: new HasTextFn() { },
            Input: [new Str() { Value = "world" }],
            ParentKey: "a",
            InputHolderKey: "a"
          ),
        ];

        var pipeline = await pipelinesContext.CreatePipeline(newUnits);
        Assert.True((await pipelinesContext.GetPipelines()).Count() == 1);
        Assert.True(pipeline.Units.Count() == 2);

        var bUnit = pipeline.Units.Where(v => v.Key == "b").First();
        Assert.True(bUnit.Parent is { } && bUnit.Parent.Key == "a");
        Assert.True(bUnit.InputHolder is { } && bUnit.InputHolder.Key == "a");
      }
    );

  [Fact]
  public async Task StartPipeline() =>
    await WithScope(
      async (scope, dbContext) =>
      {
        var pipelinesContext = scope.ServiceProvider.GetRequiredService<PipelinesContext>();

        List<NewUnitDTO> newUnits =
        [
          new(
            Key: "a",
            Fn: new InputFn { },
            Input: [new Str { Value = "hello world" }],
            ParentKey: null,
            InputHolderKey: null
          ),
          new(
            Key: "b",
            ParentKey: "a",
            InputHolderKey: "a",
            Fn: new HasSubstringFn { },
            Input: [new Str() { Value = "world" }]
          ),
          new(
            Key: "c",
            ParentKey: "b",
            InputHolderKey: "b",
            Fn: new CondFn { },
            Input:
            [
              new InputFn { Input = [new UnitReference { Key = "d1" }] },
              new InputFn { Input = [new UnitReference { Key = "d2" }] },
            ]
          ),
          new(
            Key: "d1",
            ParentKey: "c",
            InputHolderKey: "c",
            Fn: new InputFn { },
            Input: [new Str { Value = "yes" }]
          ),
          new(
            Key: "d2",
            ParentKey: "c",
            InputHolderKey: "c",
            Fn: new InputFn { },
            Input: [new Str { Value = "no" }]
          ),
        ];

        var pipeline = await pipelinesContext.CreatePipeline(newUnits);

        await pipelinesContext.StartPipeline(pipeline.Id);

        var resultPipeline = await pipelinesContext.GetPipeline(pipeline.Id);

        var lastUnit = resultPipeline!.Units.OrderByDescending(v => v.FinishedAt).FirstOrDefault();

        Assert.True(lastUnit is { Status: UnitStatus.Done } lu && lu.Key == "d1");
      }
    );
}

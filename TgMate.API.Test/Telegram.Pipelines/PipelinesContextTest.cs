using Microsoft.Extensions.DependencyInjection;
using TgMate.API.Telegram.Pipelines;
using TgMate.API.Telegram.Pipelines.FnMembers;
using TgMate.API.Telegram.Pipelines.Fns.Flow;
using TgMate.API.Telegram.Pipelines.Fns.Messages;

namespace TgMate.API.Test.Telegram.Pipelines;

public class PipelinesContextTest() : CustomLifetime(nameof(PipelinesContextTest))
{
  [Fact]
  public async Task CreatePipeline() =>
    await WithScope(
      async (scope, dbContext) =>
      {
        using var tc = await dbContext.Database.BeginTransactionAsync();

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
}

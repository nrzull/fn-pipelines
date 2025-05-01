using Microsoft.Extensions.DependencyInjection;
using TgMate.API.Telegram.Pipelines;

namespace TgMate.API.Test.Telegram.Pipelines;

public class PipelinesContextTest() : CustomLifetime(nameof(PipelinesContextTest))
{
  [Fact]
  public async Task CreatePipeline()
  {
    await using var scope = App!.Services.CreateAsyncScope();

    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    using var tc = await dbContext.Database.BeginTransactionAsync();

    var pipelinesContext = scope.ServiceProvider.GetRequiredService<PipelinesContext>();

    await pipelinesContext.CreatePipeline();

    Assert.True((await pipelinesContext.GetPipelines()).Count() == 1);
  }
}

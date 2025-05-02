using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Testcontainers.PostgreSql;

namespace TgMate.API.Test;

public class CustomLifetime(string name) : IAsyncLifetime
{
  public CustomWebApplicationFactory? App { get; set; }

  PostgreSqlContainer Db { get; set; } =
    new PostgreSqlBuilder()
      .WithImage("postgres:16-alpine")
      .WithName(name)
      .WithReuse(true)
      .WithLogger(LoggerFactory.Create(_ => { }).CreateLogger("PostgreSqlBuilder"))
      .Build();

  public async Task InitializeAsync()
  {
    await Db.StartAsync();
    App = new CustomWebApplicationFactory(Db.GetConnectionString());
    await WithScope(
      async (_scope, dbContext) =>
      {
        await dbContext.Database.EnsureCreatedAsync();
      }
    );
  }

  public Task DisposeAsync() => Db.DisposeAsync().AsTask();

  public async Task WithScope(Func<AsyncServiceScope, ApplicationDbContext, Task> cb)
  {
    await using var scope = App!.Services.CreateAsyncScope();

    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    await cb(scope, dbContext);
  }
}

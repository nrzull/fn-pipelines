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
    await App.ApplyMigrations();
  }

  public Task DisposeAsync() => Db.DisposeAsync().AsTask();
}
